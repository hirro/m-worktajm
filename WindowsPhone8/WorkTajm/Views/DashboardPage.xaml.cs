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

namespace WorkTajm.Views
{
    public partial class PanoramaPage : PhoneApplicationPage
    {
        public enum ApplicationBarType
        {
            None,
            Normal
        };

        public PanoramaPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!WorkTajmViewModel.Instance.IsLoggedIn)
            {
                WorkTajmViewModel.Instance.Login();
            }
        }

        public void SetApplicationBarType(ApplicationBarType type)
        {
            switch (type)
            {
                case ApplicationBarType.None:
                    ApplicationBar = null;
                    break;
                case ApplicationBarType.Normal:
                    ApplicationBar = normalApplicationBar;
                    break;
            }
        }

        private ApplicationBar normalApplicationBar;
        private ApplicationBar dialogApplicationBar;
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            normalApplicationBar = new ApplicationBar();
            normalApplicationBar.Opacity = 1;
            normalApplicationBar.IsVisible = true;
            normalApplicationBar.Mode = ApplicationBarMode.Default;

            // Sync button
            ApplicationBarIconButton appBarSyncButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/refresh.png", UriKind.Relative));
            appBarSyncButton.Text = AppResources.AppBarSyncButtonText;
            appBarSyncButton.Click += sync_Click;
            normalApplicationBar.Buttons.Add(appBarSyncButton);

            // Add button
            ApplicationBarIconButton appBarAddButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/add.png", UriKind.Relative));
            appBarAddButton.Text = AppResources.AppBarAddButtonText;
            appBarAddButton.Click += add_Click;
            normalApplicationBar.Buttons.Add(appBarAddButton);

            // About menu text
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarAboutText);
            normalApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += about_Click;

            // Logout menu text
            ApplicationBarMenuItem appBarLogout = new ApplicationBarMenuItem(AppResources.AppBarLogoutText);
            normalApplicationBar.MenuItems.Add(appBarLogout);
            appBarLogout.Click += logout_Click;

        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void logout_Click(object sender, EventArgs e)
        {
            WorkTajmViewModel.Instance.Logout();
        }

        private void sync_Click(object sender, EventArgs e)
        {
            //
        }

        private void add_Click(object sender, EventArgs e)
        {
            switch (this.Panorama.SelectedIndex)
            {
                case 2:
                    NavigationService.Navigate(new Uri("/Views/CustomerPage.xaml", UriKind.Relative));        
                    break;
                case 3:
                    NavigationService.Navigate(new Uri("/Views/TimeEntryPage.xaml", UriKind.Relative));        
                    break;
                default:
                    NavigationService.Navigate(new Uri("/Views/ProjectPage.xaml", UriKind.Relative));        
                    break;
            }

        }

    }
}