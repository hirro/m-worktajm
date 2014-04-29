using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.Backend
{
    public class UrlBuilder
    {
        const string HOST_CF = "http://worktajm-api.cfapps.io";
        const string HOST_AC = "http://arnellconsulting.dyndns.org:8080";

        public enum Paths
        {
            Register,
            Login,
            Projects,
            Customer,
            TimeEntry
        };

        private static readonly Dictionary<Paths, string> dictionary = new Dictionary<Paths, string>
        {
	        { Paths.Register, "registration" },
	        { Paths.Login, "authenticate" },
	        { Paths.Projects, "project" },
            { Paths.Customer, "customer" },
            { Paths.TimeEntry, "timeEntry" }
        };

        public static string Lookup(Paths value)
        {
            string result;
            dictionary.TryGetValue(value, out result);
            return result;
        }

        public static string BuildUrl(Paths path)
        {
            return String.Format("{0}/{1}", GetHost(), Lookup(path));
        }

        public static string GetHost()
        {
            return HOST_CF;
        }


    }
}
