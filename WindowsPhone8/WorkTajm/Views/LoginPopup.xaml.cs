using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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

        private void login_Click(object sender, RoutedEventArgs e)
        {
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
