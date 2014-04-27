using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.Storage
{
    /*
     * Used to store configuration properties.
     * Should be safe (saved) between update.
     */
    class Configuration
    {
        // Recommended singleton pattern in multithreaded context
        private static volatile Configuration instance;
        private static object syncRoot = new Object();
        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Configuration();
                        }
                    }
                }
                return instance;
            }
        }

        IsolatedStorageSettings store;
        
        public string Password 
        {
            get
            {
                string stringValue;
                store.TryGetValue(Constants.Constants.password, out stringValue);
                return stringValue;
            }
            set
            {
                store[Constants.Constants.password] = value;
                store.Save();
            }
        }
        public string Username 
        {
            get
            {
                string stringValue;
                store.TryGetValue(Constants.Constants.username, out stringValue);
                return stringValue;
            }
            set
            {
                store[Constants.Constants.username] = value;
                store.Save();
            }
        }
        public bool RememberMe 
        {
            get
            {
                bool boolValue;
                store.TryGetValue(Constants.Constants.rememberMe, out boolValue);
                return boolValue;
            }
            set
            {
                store[Constants.Constants.rememberMe] = value;
                store.Save();
            }
        }

        private Configuration()
        {
            store = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
        }

        internal void ForgeMe()
        {
            RememberMe = false;
            Password = null;
            Username = null;
        }
    }
}
