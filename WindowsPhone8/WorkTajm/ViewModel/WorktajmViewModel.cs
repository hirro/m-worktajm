/*
 * Copyright 2014 Jim Arnell
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTajm.DataModel;

namespace WorkTajm
{
    class WorkTajmViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database
        private WorkTajmContext workTajmDb;
        private bool isDataLoaded;

        public ObservableCollection<Customer> Customers { get; private set; }
        public ObservableCollection<Project> Projects { get; private set; }
        public ObservableCollection<TimeEntry> TimeEntries { get; private set; }

        public WorkTajmViewModel(string workTajmConnectionString)
        {
            workTajmDb = new WorkTajmContext(workTajmConnectionString);
            if (!workTajmDb.DatabaseExists())
            {
                workTajmDb.CreateDatabase();
            } 
        }

        public void LoadData()
        {
            if (!isDataLoaded)
            {
                Customers = new ObservableCollection<Customer>(workTajmDb.Customers);
                Projects = new ObservableCollection<Project>(workTajmDb.Projects);
                TimeEntries = new ObservableCollection<TimeEntry>(workTajmDb.TimeEntries);
                isDataLoaded = true;
            }
        }

        public void AddCustomer(Customer customer)
        {
            workTajmDb.Customers.InsertOnSubmit(customer);
            Customers.Add(customer);
            workTajmDb.SubmitChanges();
        }

        public void AddProject(Project project)
        {
            workTajmDb.Projects.InsertOnSubmit(project);
            Projects.Add(project);
            workTajmDb.SubmitChanges();
        }

        public void AddTimeEntry(TimeEntry timeEntry)
        {
            workTajmDb.TimeEntries.InsertOnSubmit(timeEntry);
            TimeEntries.Add(timeEntry);
            workTajmDb.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
