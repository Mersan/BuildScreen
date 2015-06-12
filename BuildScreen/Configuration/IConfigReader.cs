namespace BuildScreen.Configuration
{
    public interface IConfigReader
    {
        T GetAppSetting<T>(string setting);
    }
}