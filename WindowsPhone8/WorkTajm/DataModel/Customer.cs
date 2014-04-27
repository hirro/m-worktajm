using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace WorkTajm.DataModel
{

    [Table]
    public class Customer : INotifyPropertyChanged, INotifyPropertyChanging
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

        // Remote id
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

        // Name
        private string name;

        [Column]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                NotifyPropertyChanging();
                name = value;
                NotifyPropertyChanged();
            }
        }

        // Line1 (optional)
        private string line1;

        [Column]
        public string Line1
        {
            get
            {
                return line1;
            }
            set
            {
                NotifyPropertyChanging();
                line1 = value;
                NotifyPropertyChanged();
            }
        }

        // Line2 (optional)
        private string line2;

        [Column]
        public string Line2
        {
            get
            {
                return line2;
            }
            set
            {
                NotifyPropertyChanging();
                line2 = value;
                NotifyPropertyChanged();
            }
        }


        // zip (optional)
        private string zip;

        [Column]
        public string Zip
        {
            get
            {
                return zip;
            }
            set
            {
                NotifyPropertyChanging();
                zip = value;
                NotifyPropertyChanged();
            }
        }

        // country (optional)
        private string country;

        [Column]
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                NotifyPropertyChanging();
                country = value;
                NotifyPropertyChanged();
            }
        }

        // referencePerson (optional)
        private string referencePerson;

        [Column]
        public string ReferencePerson
        {
            get
            {
                return referencePerson;
            }
            set
            {
                NotifyPropertyChanging();
                referencePerson = value;
                NotifyPropertyChanged();
            }
        }

        // organizationNumber (optional)
        private string organizationNumber;

        [Column]
        public string OrganizationNumber
        {
            get
            {
                return organizationNumber;
            }
            set
            {
                NotifyPropertyChanging();
                organizationNumber = value;
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
