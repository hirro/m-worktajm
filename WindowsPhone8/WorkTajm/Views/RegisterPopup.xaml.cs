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
            ApplicationBar dialogApplicationBar = new ApplicationBar();
            dialogApplicationBar.Opacity = 1;
            dialogApplicationBar.IsVisible = true;
            dialogApplicationBar.Mode = ApplicationBarMode.Default;

            // Sync button
            ApplicationBarIconButton appBarRegisterButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/check.png", UriKind.Relative));
            appBarRegisterButton.Text = AppResources.AppBarRegisterButtonText;
            appBarRegisterButton.Click += register_Click;
            dialogApplicationBar.Buttons.Add(appBarRegisterButton);

            PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
            currentPage.ApplicationBar = dialogApplicationBar;
        }

        private async void register_Click(object sender, EventArgs e)
        {
            var form = (RegisterPopupControl)registerPopup.Child;

            // Send registration request
            var registered = await WorkTajmViewModel.Instance.Register(firstName.Text, lastName.Text, email.Text, password.Password);
            if (registered)
            {
                registerPopup.IsOpen = false;

                // Restore application bar
                PanoramaPage currentPage = (App.Current.RootVisual as PhoneApplicationFrame).Content as PanoramaPage;
                currentPage.ShowApplicationBar();
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
