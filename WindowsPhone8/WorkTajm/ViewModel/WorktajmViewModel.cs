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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
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

            /*
            if (workTajmDb.DatabaseExists())
            {
                workTajmDb.DeleteDatabase();
            }
             */ 
            if (!workTajmDb.DatabaseExists())
            {
                workTajmDb.CreateDatabase();
            }
            LoadData();
        }

        public void LoadData()
        {
            if (!isDataLoaded)
            {
                Customers = new ObservableCollection<Customer>(workTajmDb.Customers);
                Projects = new ObservableCollection<Project>(workTajmDb.Projects);
                TimeEntries = new ObservableCollection<TimeEntry>(workTajmDb.TimeEntries);

                Projects.Add(new Project() { ProjectName="Test Project", Description="Description", Id=33});
                /*
                using( WorkTajmContext library = new WorkTajmContext( WorkTajmContext.ConnectionString)) 
                { 
                    DataLoadOptions dlo = new DataLoadOptions(); 
                    dlo.LoadWith < Customer > (customer => customer); 
                    library.LoadOptions = dlo; 
                    Customers = library.Customers;
                }
                 */

                // Find all customers
                var loadedCustomers = from Customer b in workTajmDb.Customers
                                   orderby b.Name
                                   select b;
                var ccc = loadedCustomers.ToArray();
            
                // Find all projecs
                var loadedProjects = from Project p in workTajmDb.Projects
                                      orderby p.ProjectName
                                      select p;
                var ddd = loadedProjects.ToArray();

                // All is loaded
                isDataLoaded = true;
            }
        }


        public void AddProject(Project project)
        {
            // Check for duplicates
            var cc = from Customer b in workTajmDb.Projects where b.Id == project.Id select b;
            if (cc == null)
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
            workTajmDb.Projects.InsertOnSubmit(project);
            Projects.Add(project);
            workTajmDb.SubmitChanges();
        }

        public void AddCustomer(Customer customer)
        {
            // Check for duplicates
            var cc = from Customer b in workTajmDb.Customers where b.Id==customer.Id select b;
            if (cc == null)
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
        }

        public void AddTimeEntry(TimeEntry timeEntry)
        {
            // Check for duplicates
            var cc = from Customer b in workTajmDb.TimeEntries where b.Id == timeEntry.Id select b;
            if (cc == null)
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
                return BackendApi.Username;
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

        internal Task Authenticate(string username, string password)
        {
            return BackendApi.Authenticate(username, password);
        }

        public bool Authenticated
        { 
            get
            {
                return BackendApi.LoggedIn;
            }  
        }
    }
}
