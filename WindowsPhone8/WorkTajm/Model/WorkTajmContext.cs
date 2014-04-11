/*
 * 
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkTajm.Model
{
    class WorkTajmContext : DataContext
    {
        public WorkTajmContext(string connectionString)
            : base(connectionString)
        {
        }

        // Specify available projects
        public Table<Project> Projects;

        // Specify customers
        public Table<Customer> Customers;

        // Time entries
        public Table<TimeEntry> TimeEntries;

    }

    [Table]
    public class TimeEntry : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property, and database column.
        private int _timeEntryId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Idenitity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int TimeEntryId
        {
            get { return _timeEntryId; }
            set
            {
                if (_timeEntryId != value)
                {
                    NotifyPropertyChanging("TimeEntryId");
                    _timeEntryId = value;
                    NotifyPropertyChanged("TimeEntryId");
                }
            }
        }

        // Define project name: private field, public property, and database column.
        private string _projectmName;

        [Column]
        public string ProjectName
        {
            get { return _projectmName; }
            set
            {
                if (_projectmName != value)
                {
                    NotifyPropertyChanging("ProjectName");
                    _projectmName = value;
                    NotifyPropertyChanged("ProjectName");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class Customer : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }

    [Table]
    public class Project : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }
}
