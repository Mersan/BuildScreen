using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BuildScreen.Configuration;
using BuildScreen.Dto.Octopus;
using Octopus.Client;
using Octopus.Client.Model;
using Environment = BuildScreen.Dto.Octopus.Environment;

namespace BuildScreen.Helpers
{
    public class OctopusHelper
    {
        //https://octopus.valtech.se/api/dashboard/dynamic?projects=projects-289&includePrevious=false

        private string _defaultApiUrl = "http://buildscreen.local/api";
        private string _apiKey { get; set; }

        public string ApiUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultApiUrl))
                    _defaultApiUrl = Config.OctopusApiHost;
                return _defaultApiUrl;
            }
        }
        public string ApiKey
        {
            get
            {
                if (string.IsNullOrEmpty(_apiKey))
                    _apiKey = Config.OctopusApiKey;
                return _apiKey;
            }
        }

        public OctopusHelper()
        {
            
        }

        protected internal virtual void HttpCall(Func<HttpClient, Task<HttpResponseMessage>> Do, AuthenticationHeaderValue auth = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-Octopus-ApiKey", ApiKey);
                    httpClient.BaseAddress = new Uri(ApiUrl);
                    if (auth != null) httpClient.DefaultRequestHeaders.Authorization = auth;
                    var result = Do(httpClient);
                    result.Result.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException e)
            {
                //if it does not exists, its ok
                if (!e.Message.Contains("404"))
                    throw;
            }
            catch (Exception ex)
            {
                throw new Exception("HttpCall failed", ex);
            }
        }

        protected internal virtual T HttpResult<T>(Func<HttpClient, Task<HttpResponseMessage>> Do, AuthenticationHeaderValue auth = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-Octopus-ApiKey", ApiKey);
                    httpClient.BaseAddress = new Uri(ApiUrl);
                    if (auth != null) httpClient.DefaultRequestHeaders.Authorization = auth;
                    var result = Do(httpClient);
                    result.Result.EnsureSuccessStatusCode();

                    var res = result.Result.Content.ReadAsStringAsync().Result;

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
                            res);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static string GetEnvironmentName(Item item ,IEnumerable<Environment> environments)
        {
            if (item == null)
                return "N/A";
            foreach (var environment in environments)
            {
                if (String.Equals(item.EnvironmentId, environment.Id, StringComparison.CurrentCultureIgnoreCase))
                {
                    return environment.Name;
                }
            }
            return item.EnvironmentId;
        }




        public static IEnumerable<EnvironmentResource> GetEnvironments()
        {
            var server = Config.OctopusApiHost;
            var apiKey = Config.OctopusApiKey;
            var endpoint = new OctopusServerEndpoint(server, apiKey);
            var repository = new OctopusRepository(endpoint);
            var dashboardConfigurations = repository.DashboardConfigurations.GetDashboardConfiguration();
            var projects = repository.Projects.Get(Config.OctopusProject);
            var dashboards = repository.Dashboards.GetDynamicDashboard(new[] { Config.OctopusProject },null);


            var machines = repository.Machines.FindAll();
            var environments = new List<EnvironmentResource>();

            foreach (var env in dashboards.Environments)
            {
                var e = repository.Environments.Get(env.Id);
                environments.Add(e);
            }
                
            return environments;
        }
    }
}