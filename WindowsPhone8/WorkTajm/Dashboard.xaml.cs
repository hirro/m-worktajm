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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using WorkTajm.Backend;
using WorkTajm.Storage;

namespace WorkTajm
{
    public partial class Dashboard : PhoneApplicationPage
    {
        // Constructor
        public Dashboard()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            if (!Synchronizer.Instance.LoggedIn)
            {
                Login();
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        #region Popup
        private Popup loginPopup = new Popup();
        private void Login()
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
                var form = (LoginPopupControl) loginPopup.Child;
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
        }
        #endregion

        #region BackKey
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (loginPopup != null && loginPopup.IsOpen)
            {
                loginPopup.IsOpen = false;
                e.Cancel = true;
            }

        }

        private void PhoneApplicationPage_BackKeyPress(
            object sender, CancelEventArgs e)
        {
        }
        #endregion

    }
}