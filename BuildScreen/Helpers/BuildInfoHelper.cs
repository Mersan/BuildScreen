using System.Text.RegularExpressions;

namespace BuildScreen.Helpers
{
    public class BuildInfoHelper
    {
        public static string GetUrlFromDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                var matches = Regex.Match(description, "^*url:(.*)");

                if (matches.Success)
                {
                    return matches.Groups[1].Value;
                }
            }
            return null;
        }
    }
}