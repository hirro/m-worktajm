/*
 * Copyright 2014 Jim Arnell
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Threading;
using WorkTajm.Backend;
using WorkTajm.DataModel;
using WorkTajm.Resources;
using WorkTajm.Storage;
using WorkTajm.Views;

namespace WorkTajm
{
    class WorkTajmViewModel : INotifyPropertyChanged
    {
        private static Object lockObject = new Object();
        private static WorkTajmViewModel mainViewModel;
        public static WorkTajmViewModel Instance
        {
            get
            {
                if (mainViewModel == null)
                {
                    lock (lockObject)
                    {
                        mainViewModel = new WorkTajmViewModel(WorkTajmContext.ConnectionString);
                    }
                }

                return mainViewModel;
            }
        }

        // Backend API
        private BackendApi _backendApi = new BackendApi();
        public BackendApi BackendApi
        {
            get
            {
                return _backendApi;
            }
        }

        // LINQ to SQL data context for the local database
        private WorkTajmContext workTajmDb;
        private bool isDataLoaded;

        public ObservableCollection<Customer> Customers { get; private set; }
        public ObservableCollection<Project> Projects { get; private set; }
        public ObservableCollection<TimeEntry> TimeEntries { get; private set; }

        public WorkTajmViewModel(string workTajmConnectionString)
        {
            workTajmDb = new WorkTajmContext(workTajmConnectionString);

            if (!workTajmDb.DatabaseExists())
            {
                workTajmDb.CreateDatabase();
                workTajmDb.SubmitChanges();
            }

            // Start background thread
            ThreadPool.QueueUserWorkItem(Synchronize);
        }

        public void LoadData()
        {
            if (!isDataLoaded)
            {
                Customers = new ObservableCollection<Customer>(workTajmDb.Customers);
                Projects = new ObservableCollection<Project>(workTajmDb.Projects);
                TimeEntries = new ObservableCollection<TimeEntry>(workTajmDb.TimeEntries);

                // All is loaded
                isDataLoaded = true;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        internal async Task Authenticate(string username, string password, bool rememberMe)
        {
            BackendApi.Username = username;
            BackendApi.Password = password;
            await BackendApi.Authenticate();
            if (Authenticated)
            {
                LoadData();
                Configuration.Instance.Username = username;
                Configuration.Instance.Password = password;
            }
        }

        public bool Authenticated
        {
            get
            {
                return BackendApi.LoggedIn;
            }
        }

        internal void Logout()
        {
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = AppResources.LogoutTitle,
                Message = AppResources.LogoutText,
                LeftButtonContent = AppResources.LogoutConfirm,
                RightButtonContent = AppResources.LogoutDeny
            };

            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        OnLogout();
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Do nothing.
                        break;
                    default:
                        break;
                }
            };

            // Reset application bar
            PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;            

            messageBox.Show();
        }

        private void OnLogout()
        {
            Configuration.Instance.ForgeMe();
            workTajmDb.ResetDatabase();
            isDataLoaded = false;
            PhoneApplicationFrame NavigationService = (Application.Current.RootVisual as PhoneApplicationFrame);
            while (NavigationService.CanGoBack) 
            {
                NavigationService.RemoveBackEntry();
            }
            Login();
        }

        public bool IsLoggedIn 
        {
            get
            {
                return BackendApi.LoggedIn;
            }
        }

        public string Username
        {
            get
            {
                return Configuration.Instance.Username;
            }
        }

        public void Login()
        {
            LoginPopupControl.Show();
        }

        public async Task<bool> Register(string firstName, string lastName, string email, string password)
        {
            var registered = await BackendApi.Register(firstName, lastName, email, password);

            if (registered)  
            {
                // Reset application bar
                PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
                currentPage.ShowApplicationBar();
            }

            return registered;
        }

        internal void AddNewProject(Project project)
        {
            Projects.Add(project);
        }

        internal void AddNewCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        internal void AddNewTimeEntry(TimeEntry timeEntry)
        {
            TimeEntries.Add(timeEntry);
        }

        public void AddProject(Project project)
        {
            // Check for duplicates
            Project p = workTajmDb.Projects.FirstOrDefault(s => ((Project)s).Id == project.Id);
            if (p == null)
            {
                // Add it
                Debug.WriteLine("AddProject - New project, adding it to the database");
                workTajmDb.Projects.InsertOnSubmit(project);
                Projects.Add(project);
                workTajmDb.SubmitChanges();
            }
            else
            {
                Debug.WriteLine("AddProject - Project already exists");
            }
            NotifyPropertyChanged("Projects");
        }

        public void AddCustomer(Customer customer)
        {
            // Check for duplicates
            Customer c = workTajmDb.Customers.FirstOrDefault(s => ((Customer)s).Id == customer.Id);
            if (c == null)
            {
                // Add it
                Debug.WriteLine("AddCustomer - New customer, adding it to the database");
                workTajmDb.Customers.InsertOnSubmit(customer);
                Customers.Add(customer);
                workTajmDb.SubmitChanges();
            }
            else
            {
                Debug.WriteLine("AddCustomer - Customer already exists");
            }
            NotifyPropertyChanged("Customers");
        }

        public void AddTimeEntry(TimeEntry timeEntry)
        {
            // Check for duplicates
            TimeEntry c = workTajmDb.TimeEntries.FirstOrDefault(s => ((TimeEntry)s).Id == timeEntry.Id);
            if (c == null)
            {
                // Add it
                Debug.WriteLine("AddTimeEntry - New time entry, adding it to the database");
                workTajmDb.TimeEntries.InsertOnSubmit(timeEntry);
                TimeEntries.Add(timeEntry);
                workTajmDb.SubmitChanges();
            }
            else
            {
                Debug.WriteLine("AddTimeEntry - Time entry already exists");
            }
            NotifyPropertyChanged("TimeEntries");
        }

        private static Mutex mutex = new Mutex();
        private void Synchronize(object state)
        {
            Thread.Sleep(10 * 1000);
            while (true)
            {
                mutex.WaitOne();
                Debug.WriteLine("Synchronize - Starting");
                if (IsLoggedIn)
                {
                    SynchronizeInternal();
                }
                Thread.Sleep(30000);
                Debug.WriteLine("Synchronize - Done");
            }
        }

        private async Task SynchronizeInternal()
        {
            await SynchronzizeCustomers();
            await SynchronizeProjects();
            await SynchronizeTimeEntries();
        }

        private async Task SynchronizeTimeEntries()
        {
            // Find all customers that are new
            var newItems = from c in TimeEntries where c.Id == 0 select c;
            foreach (var timeEntry in newItems)
            {
                long newCustomerId = await BackendApi.Create(timeEntry);

                // Update customer with id
                timeEntry.Id = newCustomerId;
            }
        }

        private async Task SynchronizeProjects()
        {
            // Find all customers that are new
            var newItems = from c in Projects where c.Id == 0 select c;
            foreach (var project in newItems)
            {
                long newCustomerId = await BackendApi.Create(project);

                // Update customer with id
                project.Id = newCustomerId;
            }
        }

        private async Task SynchronzizeCustomers()
        {
            // Find all customers that are new
            var newCustomers = from c in Customers where c.Id == 0 select c;
            foreach (var customer in newCustomers)
            {
                long newCustomerId = await BackendApi.Create(customer);

                // Update customer with id
                customer.Id = newCustomerId;
            }

            // Find all customers that are modified

            // Find all customers which are to be deleted
        }

    }
}
