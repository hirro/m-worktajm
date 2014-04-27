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
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using WorkTajm.Resources;

namespace WorkTajm
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 1;
            ApplicationBar.IsVisible = false;
            ApplicationBar.Mode = ApplicationBarMode.Minimized;

            // Sync button
            ApplicationBarIconButton appBarLoginButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/check.png", UriKind.Relative));
            appBarLoginButton.Text = AppResources.AppBarLoginButtonText;
            appBarLoginButton.Click += login_Click;
            ApplicationBar.Buttons.Add(appBarLoginButton);
        }

        private void login_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/PanoramaPage.xaml", UriKind.Relative));
        }

        #region Popup
        private Popup p = new Popup();
        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (p.Child == null)
            {
                LoginPopupControl pup = new LoginPopupControl();
                pup.LoginButton.Click += new RoutedEventHandler(PopupClose_Click);
                p.Child = pup;
            }
            p.IsOpen = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void PopupClose_Click(object sender, RoutedEventArgs e)
        {
            if (p != null)
            {
                p.IsOpen = false;
            }
        }
        #endregion

        #region BackKey
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (p != null && p.IsOpen)
            {
                p.IsOpen = false;
                e.Cancel = true;
            }


            //e.Cancel = true;
            //base.OnBackKeyPress(e);
        }

        private void PhoneApplicationPage_BackKeyPress(
            object sender, CancelEventArgs e)
        {
        }
        #endregion
    }
}