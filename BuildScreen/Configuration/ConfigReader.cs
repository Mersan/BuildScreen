using System;

namespace BuildScreen.Configuration
{
    public class ConfigReader : IConfigReader
    {
        public T GetAppSetting<T>(string setting)
        {
            try
            {
                return (T)Convert.ChangeType(System.Configuration.ConfigurationManager.AppSettings[setting], typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}