using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
        const string LOGIN_URL = "http://worktajm-api.cfapps.io/authenticate";
        const string PROJECTS_URL = "http://worktajm-api.cfapps.io/project";
        const string CUSTOMER_URL = "http://worktajm-api.cfapps.io/customer";
        const string TIMEENTRY_URL = "http://worktajm-api.cfapps.io/timeEntry";

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
                    Task projectLoader = LoadFromBackend(); 
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

        public async Task LoadFromBackend()
        {
            Debug.WriteLine("LoadFromBackend");

            if (!LoggedIn)
            {
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }

            await LoadCustomers();
            await LoadProjects();
            await LoadTimeEntries();
        }

        public async Task LoadCustomers()
        {
            Debug.WriteLine("LoadCustomers");

            if (!LoggedIn)
            {
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(CUSTOMER_URL);
                webRequest.Credentials = new NetworkCredential(Username, Password);
                try
                {
                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    var txt = reader.ReadToEnd();
                    WorkTajm.DataModel.Customer[] customers = JsonConvert.DeserializeObject<WorkTajm.DataModel.Customer[]>(txt);
                    foreach (WorkTajm.DataModel.Customer customer in customers)
                    {
                        Debug.WriteLine("Added customer {0}", customer.Name);
                        WorkTajmViewModel.Instance.AddCustomer(customer);
                    }
                }
                catch (WebException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public async Task LoadProjects()
        {
            Debug.WriteLine("LoadProjects");

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
                    WorkTajm.DataModel.Project[] projects = JsonConvert.DeserializeObject<WorkTajm.DataModel.Project[]>(txt);
                    foreach (WorkTajm.DataModel.Project project in projects)
                    {
                        Debug.WriteLine("Added project {0}", project.Name);
                        WorkTajmViewModel.Instance.AddProject(project);
                    }
                }
                catch (WebException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task LoadTimeEntries()
        {
            string txt = "";

            Debug.WriteLine("LoadTimeEntries");

            if (!LoggedIn)
            {
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(TIMEENTRY_URL);
                webRequest.Credentials = new NetworkCredential(Username, Password);
                try
                {
                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    txt = reader.ReadToEnd();
                    WorkTajm.DataModel.TimeEntry[] timeEntries = JsonConvert.DeserializeObject<WorkTajm.DataModel.TimeEntry[]>(txt, new JavaScriptDateTimeConverter());
                    foreach (WorkTajm.DataModel.TimeEntry timeEntry in timeEntries)
                    {
                        Debug.WriteLine("Added time entry");
                        //WorkTajmViewModel.Instance.AddTimeEntry(timeEntry);
                    }
                }
                catch (WebException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(txt);
            }
        }
    }
}
