namespace BuildScreen.Configuration
{
    public class Config
    {
        private static IConfigReader _configReader;

        public static string TCHost
        {
            get { return ConfigReader.GetAppSetting<string>("TCHost"); }
        }
        public static bool TCUseSsl
        {
            get { return ConfigReader.GetAppSetting<bool>("TCUseSsl"); }
        }
        public static string TCUsername
        {
            get { return ConfigReader.GetAppSetting<string>("TCUsername"); }
        }
        public static string TCPassword
        {
            get { return ConfigReader.GetAppSetting<string>("TCPassword"); }
        }
        public static string TCProject
        {
            get { return ConfigReader.GetAppSetting<string>("TCProject"); }
        }
        public static string OctopusApiHost
        {
            get { return ConfigReader.GetAppSetting<string>("OctopusApiHost"); }
        }
        public static string OctopusApiKey
        {
            get { return ConfigReader.GetAppSetting<string>("OctopusApiKey"); }
        }
        public static string OctopusProject
        {
            get { return ConfigReader.GetAppSetting<string>("OctopusProject"); }
        }
        public static string BuildScreenLogo
        {
            get { return ConfigReader.GetAppSetting<string>("BuildScreenLogo"); }
        }
        public static string Dashboards
        {
            get { return ConfigReader.GetAppSetting<string>("Dashboards"); }
        }
        
        public static IConfigReader ConfigReader
        {
            get { return _configReader ?? (_configReader = new ConfigReader()); }
            set { _configReader = value; }
        }


    }
}
