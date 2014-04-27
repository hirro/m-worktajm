using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using WorkTajm.Backend.Json;
using WorkTajm.Constants;
using WorkTajm.Resources;
using WorkTajm.Storage;
using WorkTajm.ViewModel;

namespace WorkTajm.Backend
{
    /// <summary>
    /// This class handle communication to the backend.
    /// 
    /// It authenticates and synchronizes items with the database.
    /// </summary>
    class Synchronizer
    {
        const string HOST_CF = "http://worktajm-api.cfapps.io";
        const string HOST_AC = "http://arnellconsulting.dyndns.org:8080";

        const string CONTEXT_PATH = "worktajm-api/";

        const string REGISTER_PATH = "register";

        const string LOGIN_URL =        "http://worktajm-api.cfapps.io/authenticate";
        const string PROJECTS_URL =     "http://worktajm-api.cfapps.io/project";
        const string CUSTOMER_URL =     "http://worktajm-api.cfapps.io/customer";
        const string TIMEENTRY_URL =    "http://worktajm-api.cfapps.io/timeEntry";
        const string REGISTER_URL =     "http://worktajm-api.cfapps.io/registration";

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
                // Show progress on system tray
                Progress progress = new Progress();

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

            // Show progress on system tray
            Progress progress = new Progress();

            if (!LoggedIn)
            {
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }

            // Load customers
            progress.Text = "Loading customers...";
            await LoadCustomers();

            // Load projects
            progress.Text = "Loading projects...";
            await LoadProjects();

            // Load time entries
            progress.Text= "Loading time entries...";
            await LoadTimeEntries();

            // Done
            progress.Visible = false;
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
                        Debug.WriteLine("Adding customer name: [{0}], Id: [{1}]", customer.Name, customer.Id);

                        // Check that the items not already exists
                        //WorkTajmViewModel.Instance.FindCustomer(customer.Id);

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

        public async Task Register(string firstName, string lastName, string email, string password)
        {
            try
            {
                // Show progress on system tray
                Progress progress = new Progress();

                try
                {
                    using (var client = new HttpClient())
                    {
                        progress.Text = "Sending authorization request";
                        client.BaseAddress = new Uri(HOST_CF);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Create PDU
                        Registration registration = new Registration() { firstName = firstName, lastName = lastName, email = email, password = password };
                        progress.Text = "Waiting for response";
                        var response = await client.PostAsJsonAsync("registration", registration);
                        if (response.IsSuccessStatusCode)
                        {
                            // All is ok, store credentials and process to dashboard
                            Configuration.Instance.Password = password;
                            Configuration.Instance.Username = email;
                            Configuration.Instance.RememberMe = true;
                        }
                        progress.Visible=false;
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
