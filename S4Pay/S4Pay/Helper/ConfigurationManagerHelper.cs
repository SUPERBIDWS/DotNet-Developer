using System.Configuration;
using System.Diagnostics;

namespace S4Pay.Helper
{
    public class ConfigurationManagerHelper
    {
        [DebuggerStepThrough]
        public static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}