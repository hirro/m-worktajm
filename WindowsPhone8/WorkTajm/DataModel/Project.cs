using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace WorkTajm.DataModel
{
    [Table(Name="Project")]
    public class Project : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property, and database column.
        private int _internalId;
        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int InternalId
        {
            get { return _internalId; }
            set
            {
                NotifyPropertyChanging("Id2");
                _internalId = value;
                NotifyPropertyChanged("Id2");
            }
        }

        // External id
        private int _externalId;
        [Column]
        public int Id
        {
            get { return _externalId; }
            set
            {
                NotifyPropertyChanging("Id");
                _externalId = value;
                NotifyPropertyChanged("Id");
            }
        }

        // Project name
        private string projectName;
        [Column]
        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                NotifyPropertyChanging();
                projectName = value;
                NotifyPropertyChanged();
            }
        }

        // Description
        private string description;
        [Column]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                NotifyPropertyChanging();
                description = value;
                NotifyPropertyChanged();
            }
        }

        // Rate
        private decimal? rate;
        [Column]
        public decimal? Rate
        {
            get
            {
                return rate;
            }
            set
            {
                NotifyPropertyChanging();
                rate = value;
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
