using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using WorkTajm.Backend.Json;
using WorkTajm.Constants;
using WorkTajm.Resources;

namespace WorkTajm.Backend
{
    class Synchronizer
    {
        const string LOGIN_URL = "http://worktajm.arnellconsulting.dyndns.org:8080/worktajm-api/authenticate";
        const string PROJECTS_URL = "http://worktajm.arnellconsulting.dyndns.org:8080/worktajm-api/project";
        const string CUSTOMER_URL = "http://worktajm.arnellconsulting.dyndns.org:8080/worktajm-api/customer";
        const string TIMEENTRY_URL = "http://worktajm.arnellconsulting.dyndns.org:8080/worktajm-api/timeEntry";

        // Recommended singleton pattern in multithreaded context
        private static volatile Synchronizer instance;
        private static object syncRoot = new Object();
        public static Synchronizer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Synchronizer();
                        }
                    }
                }
                return instance;
            }
        }

        [DefaultValue("demo@worktajm.com")]
        public string Username { get; set; }

        [DefaultValue("")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool LoggedIn { get; set; }

        private Synchronizer()
        {
            // Hidden
        }

        public async Task Authenticate()
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(LOGIN_URL);
                webRequest.Credentials = new NetworkCredential(Username, Password);
                try
                {
                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    var txt = reader.ReadToEnd();
                    UserInformation json = JsonConvert.DeserializeObject<UserInformation>(txt);
                    LoggedIn = true;

                    // Let the projects load in its own pace
                    Task projectLoader = LoadProjects(); 
                }
                catch (WebException ex)
                {
                    MessageBox.Show(WebExceptions.Lookup(ex.Status), AppResources.LoginFailedTitle, MessageBoxButton.OK);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public async Task LoadProjects()
        {
            if (!LoggedIn)
            {
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PROJECTS_URL);
                webRequest.Credentials = new NetworkCredential(Username, Password);
                try
                {
                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    var txt = reader.ReadToEnd();
                    Project[] projects = JsonConvert.DeserializeObject<Project[]>(txt);
                    foreach (Project project in projects)
                    {

                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(WebExceptions.Lookup(ex.Status), AppResources.LoginFailedTitle, MessageBoxButton.OK);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
