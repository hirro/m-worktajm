using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.Storage
{
    class Configuration
    {
        private static Configuration instance;
        IsolatedStorageSettings store;
        private string password;
        private string username;
        private bool rememberMe;

        #region static
        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Configuration();
                }
                return instance;
            }

        }
        #endregion

        #region accessors
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        public bool RememberMe
        {
            get
            {
                return rememberMe;
            }
            set
            {
                rememberMe = value;
            }
        }
        #endregion

        private Configuration()
        {
            store = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            Load();
        }

        private void Load()
        {
            string stringValue;
            if (store.TryGetValue(Constants.Constants.username, out stringValue))
            {
                username = stringValue;
                rememberMe = true;
            }
            if (store.TryGetValue(Constants.Constants.password, out stringValue))
            {
                password = stringValue;
                rememberMe = true;
            }
        }

        private void Save()
        {
            if (rememberMe)
            {
                store[Constants.Constants.password] = password;
                store[Constants.Constants.username] = username;
            }
            else
            {
                store[Constants.Constants.password] = null;
                store[Constants.Constants.username] = null;
            }
        }

    }
}
