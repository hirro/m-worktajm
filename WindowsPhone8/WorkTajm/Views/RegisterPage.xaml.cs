using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WorkTajm.Resources;

namespace WorkTajm.Views
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 1;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Mode = ApplicationBarMode.Default;

            // Sync button
            ApplicationBarIconButton appBarSyncButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/check.png", UriKind.Relative));
            appBarSyncButton.Text = AppResources.AppBarRegisterButtonText;
            appBarSyncButton.Click += register_Click;
            ApplicationBar.Buttons.Add(appBarSyncButton);

            // About menu text
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarAboutText);
            ApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += about_Click;
        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void register_Click(object sender, EventArgs e)
        {
            // Validate

            // Send registration
            WorkTajmViewModel.Instance.Register(
                firstName.Text,
                lastName.Text,
                email.Text,
                password.Password);

            // 

        }
    
    }
}