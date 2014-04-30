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
using WorkTajm.Views;
using WorkTajm.Resources;

namespace WorkTajm
{
    public partial class LoginPopupControl : UserControl
    {
        public LoginPopupControl()
        {
            InitializeComponent();

            //get heigth and width
            double height = Application.Current.Host.Content.ActualHeight;
            double width = Application.Current.Host.Content.ActualWidth;

            this.Width = width;
            this.Height = height;

            BuildDialogApplicationBar();
        }

        private void BuildDialogApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar dialogApplicationBar = new ApplicationBar();
            dialogApplicationBar.Opacity = 1;
            dialogApplicationBar.IsVisible = true;
            dialogApplicationBar.Mode = ApplicationBarMode.Default;

            // Login (check) button
            ApplicationBarIconButton appBarSyncButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/check.png", UriKind.Relative));
            appBarSyncButton.Text = AppResources.AppBarLoginButtonText;
            appBarSyncButton.Click += check_Click;
            dialogApplicationBar.Buttons.Add(appBarSyncButton);

            // Login (check) button
            ApplicationBarIconButton appBarRegisterButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/add.png", UriKind.Relative));
            appBarRegisterButton.Text = AppResources.AppBarRegisterButtonText;
            appBarRegisterButton.Click += register_Click;
            dialogApplicationBar.Buttons.Add(appBarRegisterButton);

            PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
            currentPage.ApplicationBar = dialogApplicationBar;
        }

        private async void check_Click(object sender, EventArgs e)
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

                // Restore application bar
                PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
                currentPage.SetApplicationBarType(PanoramaPage.ApplicationBarType.Normal);
            }
        }

        private void register_Click(object sender, EventArgs e)
        {
            Hide();
            RegisterPopupControl.Show();
        }

        static private Popup loginPopup = new Popup();
        static public void Show()
        {
            // First try to login using the stored credentials
            if (loginPopup.Child == null)
            {
                loginPopup.Child = new LoginPopupControl();
            }

            LoginPopupControl pup = loginPopup.Child as LoginPopupControl;
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

            loginPopup.IsOpen = true;
        }

        static public void Hide()
        {
            loginPopup.IsOpen = false;
        }

    }
}
