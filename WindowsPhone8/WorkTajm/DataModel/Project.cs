using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace WorkTajm.DataModel
{
    [Table(Name="Project")]
    public class Project : BaseEntity<Project>
    {

        #region general properties

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public override long InternalId { get; set; }
        [Column]
        public override long Id { get; set; }
        [Column]
        public override bool Modified { get; set; }

        #endregion

        // Project name
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

        // Description
        private string _description;
        [Column]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                NotifyPropertyChanging();
                _description = value;
                NotifyPropertyChanged();
            }
        }

        // Rate
        private decimal? _rate;
        [Column]
        public decimal? Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                NotifyPropertyChanging();
                _rate = value;
                NotifyPropertyChanged();
            }
        }

    }
}
