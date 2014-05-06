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

using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Threading;
using WorkTajm.Backend;
using WorkTajm.DataModel;
using WorkTajm.Resources;
using WorkTajm.Storage;
using WorkTajm.Views;
using WorkTajm;



namespace WorkTajm.Backend
{
    class Synchronizer
    {

        public async Task SynchronizeAsync()
        {
            await SynchronzizeCustomersAsync();
            await SynchronizeProjectsAsync();
            await SynchronizeTimeEntriesAsync();
        }

        private async Task SynchronizeTimeEntriesAsync()
        {
            // Find all time entries that are new
            var newItems = from c in WorkTajmViewModel.Instance.TimeEntries where c.Id == 0 select c;
            foreach (var timeEntry in newItems)
            {
                long newCustomerId = await WorkTajmViewModel.Instance.BackendApi.Create(timeEntry);

                // Update time entry with id
                timeEntry.Id = newCustomerId;
            }

            // Find all time entries that are modified
            var modifiedTimeEntries = from c in WorkTajmViewModel.Instance.TimeEntries where c.Modified == true select c;
            foreach (var timeEntry in modifiedTimeEntries)
            {
                await WorkTajmViewModel.Instance.BackendApi.Update(timeEntry);
            }

            // Find all time entries which are to be deleted
            var deletedTimeEntries = from c in WorkTajmViewModel.Instance.TimeEntries where c.Deleted == true select c;
            foreach (var timeEntry in deletedTimeEntries)
            {
                long newCustomerId = await WorkTajmViewModel.Instance.BackendApi.Delete(timeEntry);
                WorkTajmViewModel.Instance.TimeEntries.Remove(timeEntry);
            }
        }

        private async Task SynchronizeProjectsAsync()
        {
            // Find all customers that are new
            var newItems = from c in WorkTajmViewModel.Instance.Projects where c.Id == 0 select c;
            foreach (var project in newItems)
            {
                long newCustomerId = await WorkTajmViewModel.Instance.BackendApi.Create(project);

                // Update customer with id
                project.Id = newCustomerId;
            }

            // Find all customers that are modified
            var modifiedProjects= from c in WorkTajmViewModel.Instance.Projects where c.Modified == true select c;
            foreach (var project in modifiedProjects)
            {
                await WorkTajmViewModel.Instance.BackendApi.Update(project);
            }

            // Find all customers which are to be deleted
            var deletedProjects = from c in WorkTajmViewModel.Instance.Projects where c.Deleted == true select c;
            foreach (var project in deletedProjects)
            {
                long newCustomerId = await WorkTajmViewModel.Instance.BackendApi.Delete(project);
                WorkTajmViewModel.Instance.Projects.Remove(project);
            }
        }

        private async Task SynchronzizeCustomersAsync()
        {
            // Find all customers that are new
            var newCustomers = from c in WorkTajmViewModel.Instance.Customers where c.Id == 0 select c;
            foreach (var customer in newCustomers)
            {
                long newCustomerId = await WorkTajmViewModel.Instance.BackendApi.Create(customer);

                // Update customer with id
                customer.Id = newCustomerId;
            }

            // Find all customers that are modified
            var modifiedCustomers = from c in WorkTajmViewModel.Instance.Customers where c.Modified==true select c;
            foreach (var customer in modifiedCustomers)
            {
                await WorkTajmViewModel.Instance.BackendApi.Update(customer);
            }

            // Find all customers which are to be deleted
            var deletedCustomers = from c in WorkTajmViewModel.Instance.Customers where c.Deleted == true select c;
            foreach (var customer in deletedCustomers)
            {
                await WorkTajmViewModel.Instance.BackendApi.Delete(customer);
                WorkTajmViewModel.Instance.Customers.Remove(customer);
            }
        }

    }
}
