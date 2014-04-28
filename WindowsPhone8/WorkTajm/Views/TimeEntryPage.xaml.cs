using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WorkTajm.DataModel;
using WorkTajm.Resources;

namespace WorkTajm.Views
{
    public partial class TimeEntryPage : PhoneApplicationPage
    {
        public TimeEntryPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Opacity = 1;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Mode = ApplicationBarMode.Default;

            // OK button
            ApplicationBarIconButton appBarOkButton = new ApplicationBarIconButton(new Uri("/Assets/Icons/SDK8/Light/check.png", UriKind.Relative));
            appBarOkButton.Text = AppResources.AppBarAddButtonText;
            appBarOkButton.Click += ok_Click;
            ApplicationBar.Buttons.Add(appBarOkButton);
        }

        private void ok_Click(object sender, EventArgs e)
        {
            TimeEntry timeEntry = new TimeEntry();
            timeEntry.ProjectName = projectName.Text;
            WorkTajmViewModel.Instance.AddNewTimeEntry(timeEntry);

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}