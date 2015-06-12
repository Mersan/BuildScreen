using System;
using System.Collections.Generic;
using System.Linq;
using BuildScreen.Configuration;
using BuildScreen.Dto.Octopus;
using BuildScreen.Helpers;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace BuildScreen.ViewModels
{
    public class BuildStatusViewModel
    {
        private TeamCityClient _client { get; set; }
        public string TCProject { get; set; }
        public IEnumerable<Build> Builds { get; set; }
        public Server Serverinfo { get; set; }
        public List<string> RunningBuildTypeIds { get; set; }
        public Dashboard OctopusDashboard { get; set; }
        public Dictionary<string, string> Dashboards { get; set; }
        public List<string> ErrorMessage { get; set; }

        public BuildStatusViewModel(string tcProject = null, string octopusProject = null)
        {
            _client = new TeamCityClient(Config.TCHost, Config.TCUseSsl);
            _client.Connect(Config.TCUsername, Config.TCPassword);
            TCProject = tcProject ?? Config.TCProject;
            ErrorMessage = new List<string>();
            try
            {
                Serverinfo = _client.ServerInformation.ServerInfo();
                RunningBuildTypeIds = new List<string>();
                RunningBuildTypeIds.AddRange(GetRunningBuilds());
                Builds = GetBuildConfigs();
            }
            catch (Exception ex)
            {
                Serverinfo = null;
                ErrorMessage.Add("TeamCity data not available. " + ex.Message);
            }

            try
            {
                OctopusDashboard = GetOctopusDashBoard();
            }
            catch (Exception ex)
            {
                OctopusDashboard = null;
                ErrorMessage.Add("Octopus data not available. " + ex.Message);
            }
            Dashboards = GetDashboards();
        }

        private Dictionary<string, string> GetDashboards()
        {
            var dashboardsString = Config.Dashboards;
            if (string.IsNullOrEmpty(dashboardsString))
                return null;
            var dashboardsInfo = dashboardsString.Split(';');
            var dashboards = new Dictionary<string, string>();
            foreach (var d in dashboardsInfo)
            {
                var split = d.Split('|');
                if(split.Length == 2)
                    dashboards.Add(split[0],split[1]);
            }
            return dashboards;
        }

        private Dashboard GetOctopusDashBoard()
        {
            var env = OctopusHelper.GetEnvironments();
            var octoHelper = new OctopusHelper();
            var url = Config.OctopusApiHost + "dashboard/dynamic?projects="+Config.OctopusProject+"&includePrevious=false";
            var dashBoard = octoHelper.HttpResult<Dashboard>(client => client.GetAsync(url));
            return dashBoard;
        }

        public List<string> GetRunningBuilds()
        {
            var bl = BuildLocator.RunningBuilds();
            var builds = _client.Builds.ByBuildLocator(bl);
            var str = new List<string>();
            foreach (var build in builds)
            {
                str.Add(build.BuildTypeId);
            }
            return str;
        }

        public IEnumerable<Build> GetBuildConfigs()
        {

            if (!string.IsNullOrEmpty(TCProject))
            {
                var buildConfigs = _client.BuildConfigs.ByProjectId(TCProject);

                var builds = new List<Build>();
                foreach (var buildConfig in buildConfigs)
                {
                    var build = _client.Builds.LastBuildByBuildConfigId(buildConfig.Id);
                    if (build == null)
                    {
                        build = new Build();
                        build.BuildTypeId = buildConfig.Name;
                    }
                    if (RunningBuildTypeIds != null && RunningBuildTypeIds.Contains(build.BuildTypeId))
                        build.Running_info = new Running_info(){CurrentStageText = "Building"};
                    build.BuildConfig = buildConfig;
                    PopulateChanges(buildConfig, build);
                    builds.Add(build);
                }
                return builds.OrderByDescending(x=>
                {
                    var firstOrDefault = x.Changes.Change.FirstOrDefault();
                    return firstOrDefault != null ? firstOrDefault.Date : DateTime.MinValue;
                });
            }
            return null;
        }

        private void PopulateChanges(BuildConfig buildConfig, Build build)
        {
            var changes = _client.Changes.ByBuildConfigId(buildConfig.Id);
            if (changes != null && changes.Any())
            {
                build.Changes = new ChangeWrapper();
                build.Changes.Change = new List<Change>();
                foreach (var change in changes.OrderByDescending(x => x.Date))
                {
                    build.Changes.Change.Add(change);
                }
            }
        }
    }
}