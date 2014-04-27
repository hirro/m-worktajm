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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkTajm.DataModel
{
    class WorkTajmContext : DataContext
    {
        public static string ConnectionString = "Data source=isostore:/worktajm.sdf";

        public WorkTajmContext(string connectionString)
            : base(connectionString)
        {
        }

        // Specify available projects
        public Table<Project> Projects;

        // Specify customers
        public Table<Customer> Customers;

        // Time entries
        public Table<TimeEntry> TimeEntries;


        internal void ResetDatabase()
        {
            DeleteDatabase();
            CreateDatabase();
        }
    }

}
