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

        // LINQ to SQL data context for the local database
        private WorkTajmContext workTajmDb;
        private bool isDataLoaded;

        public ObservableCollection<Customer> Customers { get; private set; }
        public ObservableCollection<Project> Projects { get; private set; }
        public ObservableCollection<TimeEntry> TimeEntries { get; private set; }

        public WorkTajmViewModel(string workTajmConnectionString)
        {
            workTajmDb = new WorkTajmContext(workTajmConnectionString);

            if (workTajmDb.DatabaseExists())
            {
                workTajmDb.DeleteDatabase();
            }
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
                isDataLoaded = true;
            }
        }


        public void AddProject(Project project)
        {
            workTajmDb.Projects.InsertOnSubmit(project);
            Projects.Add(project);
            workTajmDb.SubmitChanges();
        }

        public void AddCustomer(Customer customer)
        {
            workTajmDb.Customers.InsertOnSubmit(customer);
            Customers.Add(customer);
            workTajmDb.SubmitChanges();
        }

        public void AddTimeEntry(TimeEntry timeEntry)
        {
            workTajmDb.TimeEntries.InsertOnSubmit(timeEntry);
            TimeEntries.Add(timeEntry);
            workTajmDb.SubmitChanges();
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
            currentPage.SetApplicationBarType(PanoramaPage.ApplicationBarType.None);

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
                return Synchronizer.Instance.LoggedIn;
            }
        }

        public void Login()
        {
            LoginPopupControl.Show();
        }

        public async Task<bool> Register(string firstName, string lastName, string email, string password)
        {
            var registered = await Synchronizer.Instance.Register(firstName, lastName, email, password);

            if (registered)
            {
                // Reset application bar
                PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
                currentPage.SetApplicationBarType(PanoramaPage.ApplicationBarType.Normal);
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
    }
}
