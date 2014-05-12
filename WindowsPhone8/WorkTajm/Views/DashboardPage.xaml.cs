using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using WorkTajm.Storage;
using WorkTajm.Backend;
using WorkTajm.Resources;
using System.Diagnostics;
using WorkTajm.DataModel;

namespace WorkTajm.Views
{
    public partial class PanoramaPage : PhoneApplicationPage
    {

        public PanoramaPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();


            // Setup data bindsing
            this.DataContext = WorkTajmViewModel.Instance;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!WorkTajmViewModel.Instance.IsLoggedIn)
            {
                WorkTajmViewModel.Instance.Login();
            }
        }

        public void ShowApplicationBar()
        {
            ApplicationBar = normalApplicationBar;
        }

        private ApplicationBar normalApplicationBar;
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            normalApplicationBar = new ApplicationBar();
            normalApplicationBar.Opacity = 1;
            normalApplicationBar.IsVisible = true;
            normalApplicationBar.Mode = ApplicationBarMode.Minimized;

            // Add new project
            ApplicationBarMenuItem addProject = new ApplicationBarMenuItem(AppResources.AppBarAddProjectText);
            normalApplicationBar.MenuItems.Add(addProject);
            addProject.Click += add_ProjectClick;

            // Add new customer
            ApplicationBarMenuItem addCustomer= new ApplicationBarMenuItem(AppResources.AppBarAddCustomerText);
            normalApplicationBar.MenuItems.Add(addCustomer);
            addCustomer.Click += add_CustomerClick;

            // Add new time entry
            ApplicationBarMenuItem addTimeEntry = new ApplicationBarMenuItem(AppResources.AppBarAddTimeEntryText);
            normalApplicationBar.MenuItems.Add(addTimeEntry);
            addTimeEntry.Click += add_TimeEntryClick;

            // About menu text
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarAboutText);
            normalApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += about_Click;

            // Settings menu text
            ApplicationBarMenuItem appBarSettings = new ApplicationBarMenuItem(AppResources.AppBarSettingsText);
            normalApplicationBar.MenuItems.Add(appBarSettings);
            appBarSettings.Click += settings_Click;

            // Logout menu text
            ApplicationBarMenuItem appBarLogout = new ApplicationBarMenuItem(AppResources.AppBarLogoutText);
            normalApplicationBar.MenuItems.Add(appBarLogout);
            appBarLogout.Click += logout_Click;

        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void logout_Click(object sender, EventArgs e)
        {
            WorkTajmViewModel.Instance.Logout();
        }

        private void add_ProjectClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ProjectPage.xaml", UriKind.Relative));
        }

        private void add_CustomerClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/CustomerPage.xaml", UriKind.Relative));
        }

        private void add_TimeEntryClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/TimeEntryPage.xaml", UriKind.Relative));
        }

        private void projectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project selectedProject = (Project) ((ListBox) sender).SelectedItem;
            Debug.WriteLine("projectsListBox_SelectionChanged");

            if (selectedProject == null) return;

            AddEditProjectControl editControl = new AddEditProjectControl(selectedProject);

            Popup addPopup = new Popup();
            addPopup.Child = editControl;

            editControl.Canceled += (object cancelSender, EventArgs ea) => { addPopup.IsOpen = false; };
            editControl.Saved += (object saveSender, EventArgs ea) =>
            {
                //App.MainViewModel.UpdateBook(selectedBook, editControl.Title, editControl.Author);
                addPopup.IsOpen = false;
            };

            addPopup.IsOpen = true;
        }

        private void customersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = (Customer)((ListBox)sender).SelectedItem;
            Debug.WriteLine("customersListBox_SelectionChanged");

            if (selectedCustomer == null) return;

            AddEditCustomerControl editControl = new AddEditCustomerControl(selectedCustomer);

            Popup addPopup = new Popup();
            addPopup.Child = editControl;

            editControl.Canceled += (object cancelSender, EventArgs ea) => { addPopup.IsOpen = false; };
            editControl.Saved += (object saveSender, EventArgs ea) =>
            {
                //App.MainViewModel.UpdateBook(selectedBook, editControl.Title, editControl.Author);
                addPopup.IsOpen = false;
            };

            addPopup.IsOpen = true;
        }


    }
}