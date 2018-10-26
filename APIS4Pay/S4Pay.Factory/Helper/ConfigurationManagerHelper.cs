using System.Configuration;
using System.Diagnostics;

namespace S4Pay.Factory.Helper
{
    public static class ConfigurationManagerHelper
    {
        [DebuggerStepThrough]
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}