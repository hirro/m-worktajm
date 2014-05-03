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
        public override int InternalId { get; set; }
        [Column]
        public override int Id { get; set; }
        [Column]
        public override bool Modified { get; set; }

        #endregion

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

    }
}
