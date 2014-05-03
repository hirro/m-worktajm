using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.DataModel
{
    public abstract class BaseEntity<T> : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Didn't find any slick way to implement this without the need to duplicate column defintions in implementation class.
        #region General properties

        /// <summary>
        /// The internal database idenfier, real primary key.
        /// </summary>
        public virtual int InternalId { get; set; }

        /// <summary>
        /// The remote id of the database entry.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Object exists at remote but is modified.
        /// </summary>
        public virtual bool Modified { get; set; }
       
        /// <summary>
        /// Object is new and unsynchronized.
        /// </summary>
        public bool IsNew
        {
            get
            {
                return Id == -1;
            }
        }

        #endregion


        #region Property Change Handling

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected void NotifyPropertyChanging([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
