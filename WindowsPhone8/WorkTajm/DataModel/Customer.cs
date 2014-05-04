using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace WorkTajm.DataModel
{

    [Table]
    public class Customer : BaseEntity<Customer>
    {
        #region general properties

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public override long InternalId { get; set; }
        [Column]
        public override long Id { get; set; }
        [Column]
        public override bool Modified { get; set; }

        #endregion

        // Name
        private string _name;

        [Column]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                NotifyPropertyChanging();
                _name = value;
                NotifyPropertyChanged();
            }
        }

        // Line1 (optional)
        private string _line1;

        [Column]
        public string Line1
        {
            get
            {
                return _line1;
            }
            set
            {
                NotifyPropertyChanging();
                _line1 = value;
                NotifyPropertyChanged();
            }
        }

        // Line2 (optional)
        private string _line2;

        [Column]
        public string Line2
        {
            get
            {
                return _line2;
            }
            set
            {
                NotifyPropertyChanging();
                _line2 = value;
                NotifyPropertyChanged();
            }
        }


        // zip (optional)
        private string _zip;

        [Column]
        public string Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                NotifyPropertyChanging();
                _zip = value;
                NotifyPropertyChanged();
            }
        }

        // country (optional)
        private string _country;

        [Column]
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                NotifyPropertyChanging();
                _country = value;
                NotifyPropertyChanged();
            }
        }

        // referencePerson (optional)
        private string _referencePerson;

        [Column]
        public string ReferencePerson
        {
            get
            {
                return _referencePerson;
            }
            set
            {
                NotifyPropertyChanging();
                _referencePerson = value;
                NotifyPropertyChanged();
            }
        }

        // organizationNumber (optional)
        private string _organizationNumber;

        [Column]
        public string OrganizationNumber
        {
            get
            {
                return _organizationNumber;
            }
            set
            {
                NotifyPropertyChanging();
                _organizationNumber = value;
                NotifyPropertyChanged();
            }
        }

    }
}
