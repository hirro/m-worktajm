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
using System.Runtime.CompilerServices;
using WorkTajm.DataModel;

namespace WorkTajm.Views
{
    public partial class AddEditProjectControl : UserControl, INotifyPropertyChanged
    {
        public delegate void SaveEventHandler(object sender, EventArgs e);
        public event SaveEventHandler Saved;

        public delegate void CancelEventHandler(object sender, EventArgs e);
        public event CancelEventHandler Canceled;

        public AddEditProjectControl()
        {
            InitializeComponent();
            Resize();
        }

        private void Resize()
        {
            //get heigth and width
            double height = Application.Current.Host.Content.ActualHeight;
            double width = Application.Current.Host.Content.ActualWidth;
            this.Width = width;
            this.Height = height;
        }

        public AddEditProjectControl(Project project)
        {
            InitializeComponent();
            Resize();

            Name = project.Name;
            Description = project.Description;
        }

        #region Properties

        private string _name;
        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

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
    }
}
