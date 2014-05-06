using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace WorkTajm.DataModel
{
    [Table]
    public class TimeEntry : BaseEntity<Customer>
    {

        #region general properties

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public override long InternalId { get; set; }
        [Column]
        public override long Id { get; set; }
        [Column]
        public override bool Modified { get; set; }
        [Column]
        public override bool Deleted { get; set; }

        #endregion

        // Define project name: private field, public property, and database column.
        private string _projectName;

        [Column]
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (_projectName != value)
                {
                    NotifyPropertyChanging("ProjectName");
                    _projectName = value;
                    NotifyPropertyChanged("ProjectName");
                }
            }
        }

        // StartTime (not NULL)
        private DateTime _startTime;

        [Column]
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                NotifyPropertyChanging();
                _startTime = value;
                NotifyPropertyChanged();
            }
        }

        // EndTime
        private DateTime? _endTime;

        [Column]
        public DateTime? EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                NotifyPropertyChanging();
                _endTime = value;
                NotifyPropertyChanged();
            }
        }

        // Is active
        public bool Active
        {
            get
            {
                return _endTime == null;
            }
        }
    }
}
