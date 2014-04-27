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
            //NavigationService.GoBack();
            //NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.Relative));
        }

        public bool IsLoggedIn 
        {
            get
            {
                return Synchronizer.Instance.LoggedIn;
            }
        }

        #region Popup
        private Popup loginPopup = new Popup();
        public void Login()
        {
            // First try to login using the stored credentials
            if (loginPopup.Child == null)
            {
                LoginPopupControl pup = new LoginPopupControl();
                if (Configuration.Instance.RememberMe)
                {
                    pup.username.Text = Configuration.Instance.Username;
                    pup.password.Password = Configuration.Instance.Password;
                    pup.rememberMe.IsChecked = true;
                }
                else
                {
                    pup.rememberMe.IsChecked = false;
                }
                pup.LoginButton.Click += new RoutedEventHandler(PopupLogin_Click);
                loginPopup.Child = pup;
            }
            loginPopup.IsOpen = true;
        }

        private async void PopupLogin_Click(object sender, RoutedEventArgs e)
        {
            if (loginPopup != null)
            {
                var form = (LoginPopupControl)loginPopup.Child;
                Synchronizer.Instance.Password = form.password.Password;
                Synchronizer.Instance.Username = form.username.Text;
                await Synchronizer.Instance.Authenticate();
                if (Synchronizer.Instance.LoggedIn)
                {
                    loginPopup.IsOpen = false;
                    if (form.rememberMe.IsChecked.Value)
                    {
                        Configuration.Instance.Password = form.password.Password;
                        Configuration.Instance.Username = form.username.Text;
                        Configuration.Instance.RememberMe = true;
                    }
                    else
                    {
                        Configuration.Instance.Password = "";
                        Configuration.Instance.Username = "";
                        Configuration.Instance.RememberMe = false;
                    }
                }
            }
            loginPopup.Child = null;
        }
        #endregion

    }
}
