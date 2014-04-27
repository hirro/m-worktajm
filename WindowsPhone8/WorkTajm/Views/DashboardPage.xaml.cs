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
        public PanoramaPage()
        {
            InitializeComponent();

            if (!WorkTajmViewModel.Instance.IsLoggedIn)
            {
                WorkTajmViewModel.Instance.Login();
            }

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 1;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Mode = ApplicationBarMode.Minimized;

            // Sync button
            ApplicationBarIconButton appBarSyncButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/refresh.png", UriKind.Relative));
            appBarSyncButton.Text = AppResources.AppBarSyncButtonText;
            appBarSyncButton.Click += sync_Click;
            ApplicationBar.Buttons.Add(appBarSyncButton);

            // Add button
            ApplicationBarIconButton appBarAddButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/add.png", UriKind.Relative));
            appBarAddButton.Text = AppResources.AppBarAddButtonText;
            appBarAddButton.Click += add_Click;
            ApplicationBar.Buttons.Add(appBarAddButton);

            // About menu text
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarAboutText);
            ApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += about_Click;

            // Logout menu text
            ApplicationBarMenuItem appBarLogout = new ApplicationBarMenuItem(AppResources.AppBarLogoutText);
            ApplicationBar.MenuItems.Add(appBarLogout);
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
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}