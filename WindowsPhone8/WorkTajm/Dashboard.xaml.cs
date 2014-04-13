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
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

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
            Login();
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
            if (loginPopup.Child == null)
            {
                LoginPopupControl pup = new LoginPopupControl();
                pup.LoginButton.Click += new RoutedEventHandler(PopupLogin_Click);
                loginPopup.Child = pup;
            }
            loginPopup.IsOpen = true;
        }


        private void PopupLogin_Click(object sender, RoutedEventArgs e)
        {
            if (loginPopup != null)
            {
                Login("demo@worktajm.com", "password");
            }
        }
        //const string LOGIN_URL = "http://dev.windowsphone.com";
        const string LOGIN_URL = "http://192.168.1.94:8080/worktajm-api/authenticate";
        private async void Login(string username, string password)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(LOGIN_URL);
                //webRequest.ContentType = "text/xml";
                webRequest.Credentials = new NetworkCredential("demo@worktajm.com", "password");
                try
                {
                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    var txt = reader.ReadToEnd();
                    loginPopup.IsOpen = false; 
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
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