using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WorkTajm.Views
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            NumberOfCustomers.Text = Convert.ToString(WorkTajmViewModel.Instance.Customers.Count);
            NumberOfProjects.Text = Convert.ToString(WorkTajmViewModel.Instance.Projects.Count);
            NumberOfTimeEntries.Text = Convert.ToString(WorkTajmViewModel.Instance.TimeEntries.Count);
            UserName.Text = WorkTajmViewModel.Instance.Username;
        }
    }
}