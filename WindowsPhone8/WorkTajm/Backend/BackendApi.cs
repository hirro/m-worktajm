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
    class BackendApi
    {
        [DefaultValue("demo@worktajm.com")]
        public string Username { get; set; }

        [DefaultValue("")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool LoggedIn { get; set; }

        public BackendApi()
        {
            // Hidden
        }

        public async Task Authenticate()
        {
            try
            {
                Debug.WriteLine("Authenticate");

                // Show progress on system tray
                Progress progress = new Progress();

                string url = UrlBuilder.BuildUrl(UrlBuilder.Paths.Login);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
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
                    Task projectLoader = LoadEntriesFromBackend(); 
                }
                catch (WebException ex)
                {
                    Debug.WriteLine("Authentication failed: {0}", ex.ToString());
                    HttpWebResponse response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        MessageBox.Show(HttpStatusCodeHelper.Lookup(response.StatusCode), AppResources.LoginFailedTitle, MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show(WebExceptionStatusHelper.Lookup(ex.Status), AppResources.LoginFailedTitle, MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("!Authentication failed: {0}", e.ToString());
            }
        }

        public async Task LoadEntriesFromBackend()
        {
            Debug.WriteLine("LoadFromBackend");

            // Show progress on system tray
            Progress progress = new Progress();

            if (!LoggedIn)
            {
                Debug.WriteLine("Not logged in!");
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }

            // Load customers
            progress.Text = "Loading customers...";
            await GetCustomers();

            // Load projects
            progress.Text = "Loading projects...";
            await GetProjects();

            // Load time entries
            progress.Text= "Loading time entries...";
            //await LoadTimeEntries();

            // Done
            progress.Visible = false;
        }

        public async Task GetCustomers()
        {
            Debug.WriteLine("LoadCustomers");

            if (!LoggedIn)
            {
                Debug.WriteLine("LoadCustomers - Not logged in");
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }
            try
            {
                string url = UrlBuilder.BuildUrl(UrlBuilder.Paths.Customer);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url); 
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
                        WorkTajmViewModel.Instance.AddCustomer(customer);
                    }
                    Debug.WriteLine("LoadCustomers complete, {0} customers found", customers.Length);
                }
                catch (WebException ex)
                {
                    Debug.WriteLine("LoadCustomers failed: {0}", ex.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("!LoadCustomers failed: {0}", e.ToString());
            }
        }

        public async Task GetProjects()
        {
            Debug.WriteLine("LoadProjects");

            if (!LoggedIn)
            {
                Debug.WriteLine("LoadProjects - Not logged in");
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }
            try
            {
                string url = UrlBuilder.BuildUrl(UrlBuilder.Paths.Projects);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
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
                    Debug.WriteLine("LoadProjects complete, {0} projects found", projects.Length);
                }
                catch (WebException ex)
                {
                    Debug.WriteLine("LoadProjects failed: {0}", ex.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("!LoadProjects failed: {0}", e.ToString());
            }
        }

        public async Task GetTimeEntries()
        {
            string txt = "";

            Debug.WriteLine("LoadTimeEntries");

            if (!LoggedIn)
            {
                Debug.WriteLine("LoadTimeEntries - Not logged in");
                throw new UnauthorizedAccessException("Tried to load projects when not logged in");
            }
            try
            {
                string url = UrlBuilder.BuildUrl(UrlBuilder.Paths.TimeEntry);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
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
                    Debug.WriteLine("LoadTimeEntries complete, {0} projects found", timeEntries.Length);
                }
                catch (WebException ex)
                {
                    Debug.WriteLine("LoadTimeEntries failed: {0}", ex.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("!LoadTimeEntries failed: {0}", e.ToString());
            }
        }

        public async Task<bool> Register(string firstName, string lastName, string email, string password)
        {
            bool result = false;

            Debug.WriteLine("Register");

            // Show progress on system tray
            Progress progress = new Progress();

            try
            {
                using (var client = new HttpClient())
                {
                    progress.Text = "Sending authorization request";
                    client.BaseAddress = new Uri(UrlBuilder.GetHost());
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
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Registration failed", AppResources.RegistrationFailedTitle, MessageBoxButton.OK);
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Register failed: {0}", ex.ToString());
                MessageBox.Show(WebExceptionStatusHelper.Lookup(ex.Status), AppResources.LoginFailedTitle, MessageBoxButton.OK);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Register failed: {0}", e.ToString());
            }
            finally
            {
                progress.Visible = false;
            }
            return result;
        }


        internal async Task<long> Create(DataModel.Customer customer)
        {
            Debug.WriteLine("Create customer, name: {0}", customer.Name);

            try
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(Username, Password) })
                {

                    using (var client = new HttpClient(handler))
                    {
                        client.BaseAddress = new Uri(UrlBuilder.GetHost());
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                        // Create PDU
                        Json.Customer customerJson = new Json.Customer()
                        {
                            name = customer.Name,
                            line1 = customer.Line1,
                            line2 = customer.Line2,
                            country = customer.Country,
                            vatRegistrationNumber = customer.OrganizationNumber,
                            referencePerson = customer.ReferencePerson,
                            zip = customer.Zip
                        };
                        var response = await client.PostAsJsonAsync("customer", customerJson);
                        if (response.IsSuccessStatusCode)
                        {
                            string txt = await response.Content.ReadAsStringAsync();
                            WorkTajm.DataModel.Customer newItem = JsonConvert.DeserializeObject<WorkTajm.DataModel.Customer>(txt);
                            return newItem.Id;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Create customer failed: {0}", ex.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Create customer failed: {0}", e.ToString());
            }
            finally
            {
            }

            // This will make sure no other attempts will be made for failed create.
            return -1;            
        }


        internal async Task<long> Create(DataModel.Project project)
        {
            Debug.WriteLine("Create project, name: {0}", project.Name);

            try
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(Username, Password) })
                {

                    using (var client = new HttpClient(handler))
                    {
                        client.BaseAddress = new Uri(UrlBuilder.GetHost());
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                        // Create PDU
                        Json.Project projectJson = new Json.Project()
                        {
                            name = project.Name,
                            description = project.Description,
                        };
                        var response = await client.PostAsJsonAsync("project", projectJson);
                        if (response.IsSuccessStatusCode)
                        {
                            string txt = await response.Content.ReadAsStringAsync();
                            WorkTajm.DataModel.Project newItem = JsonConvert.DeserializeObject<WorkTajm.DataModel.Project>(txt);
                            return newItem.Id;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Create customer failed: {0}", ex.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Create customer failed: {0}", e.ToString());
            }
            finally
            {
            }

            // This will make sure no other attempts will be made for failed create.
            return -1;
        }

        internal async Task<long> Create(DataModel.TimeEntry timeEntry)
        {
            Debug.WriteLine("Create time entry");
            return -1L;
        }

        internal Task Delete(DataModel.Customer customer)
        {
            Debug.WriteLine("Delete customer, name: {0}", customer.Name);
            return null;
        }

        internal Task Update(DataModel.Customer customer)
        {
            Debug.WriteLine("Update customer, name: {0}", customer.Name);
            return null;
        }

        internal Task<long> Delete(DataModel.Project project)
        {
            Debug.WriteLine("Delete customer, name: {0}", project.Name);
            return null;
        }

        internal Task Update(DataModel.Project project)
        {
            Debug.WriteLine("Update project, name: {0}", project.Name);
            return null;
        }

        internal Task<long> Delete(DataModel.TimeEntry timeEntry)
        {
            Debug.WriteLine("Delete time entry");
            return null;
        }

        internal Task Update(DataModel.TimeEntry timeEntry)
        {
            Debug.WriteLine("Update time entry");
            return null;
        }
    }
}
