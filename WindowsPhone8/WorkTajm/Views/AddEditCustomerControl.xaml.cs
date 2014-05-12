using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using WorkTajm.DataModel;
using System.Runtime.CompilerServices;

namespace WorkTajm.Views
{
    public partial class AddEditCustomerControl : UserControl, INotifyPropertyChanged
    {
        public delegate void SaveEventHandler(object sender, EventArgs e);
        public event SaveEventHandler Saved;

        public delegate void CancelEventHandler(object sender, EventArgs e);
        public event CancelEventHandler Canceled;

        public AddEditCustomerControl()
        {
            InitializeComponent();
            Resize();
            this.DataContext = this;
        }

        public AddEditCustomerControl(Customer currentCustomer)
        {
            InitializeComponent();
            Resize();
            this.DataContext = this;

            CustomerName = currentCustomer.Name;
            OrganizationalNumber = currentCustomer.OrganizationNumber;
            Line1 = currentCustomer.Line1;
            Line2 = currentCustomer.Line2;
            ReferencePerson = currentCustomer.ReferencePerson;
            Zip = currentCustomer.Zip;
            Country = currentCustomer.Country;
        }

        private void Resize()
        {
            //get heigth and width
            double height = Application.Current.Host.Content.ActualHeight;
            double width = Application.Current.Host.Content.ActualWidth;
            this.Width = width;
            this.Height = height;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Saved != null)
            {
                Saved(this, EventArgs.Empty);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Canceled != null)
            {
                Canceled(this, EventArgs.Empty);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Properties

        public string CustomerName { get; set; }

        public string OrganizationalNumber { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string ReferencePerson { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        #endregion
    }
}
