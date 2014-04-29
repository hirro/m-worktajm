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

        }

        private async void login_Click(object sender, RoutedEventArgs e)
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

        private void register_Click(object sender, RoutedEventArgs e)
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
