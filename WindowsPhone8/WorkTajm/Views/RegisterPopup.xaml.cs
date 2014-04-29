using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WorkTajm.Backend;
using WorkTajm.Storage;
using System.Windows.Controls.Primitives;
using WorkTajm.Resources;

namespace WorkTajm.Views
{
    public partial class RegisterPopupControl : UserControl
    {
        public RegisterPopupControl()
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
            ApplicationBar dialogApplicationBar  = new ApplicationBar();
            dialogApplicationBar.Opacity = 1;
            dialogApplicationBar.IsVisible = true;
            dialogApplicationBar.Mode = ApplicationBarMode.Default;

            // Sync button
            ApplicationBarIconButton appBarSyncButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/check.png", UriKind.Relative));
            appBarSyncButton.Text = AppResources.AppBarSyncButtonText;
            appBarSyncButton.Click += register_Click;
            dialogApplicationBar.Buttons.Add(appBarSyncButton);

            PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
            currentPage.ApplicationBar = dialogApplicationBar;
        }

        private async void register_Click(object sender, EventArgs e)
        {
            var form = (LoginPopupControl)registerPopup.Child;
            Synchronizer.Instance.Password = form.password.Password;
            Synchronizer.Instance.Username = form.username.Text;
            await Synchronizer.Instance.Authenticate();
            if (Synchronizer.Instance.LoggedIn)
            {
                registerPopup.IsOpen = false;
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



        static private Popup registerPopup = new Popup();
        static public void Show()
        {
            if (registerPopup.Child == null)
            {
                registerPopup.Child = new RegisterPopupControl();
            }
            registerPopup.IsOpen = true;
        }

        static public void Hide()
        {
            registerPopup.IsOpen = false;
        }
    }
}
