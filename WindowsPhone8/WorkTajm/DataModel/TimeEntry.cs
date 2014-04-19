﻿using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace WorkTajm.DataModel
{
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

        // StartTime (not NULL)
        private DateTime startTime;

        [Column]
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                NotifyPropertyChanging();
                startTime = value;
                NotifyPropertyChanged();
            }
        }

        // EndTime
        private DateTime? endTime;

        [Column]
        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                NotifyPropertyChanging();
                endTime = value;
                NotifyPropertyChanged();
            }
        }

        // Modified
        private DateTime? modified;

        [Column]
        public DateTime? Modified
        {
            get
            {
                return modified;
            }
            set
            {
                NotifyPropertyChanging();
                modified = value;
                NotifyPropertyChanged();
            }
        }

        #region Property Change Handling

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
