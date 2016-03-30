using System;
using System.Configuration;

namespace SuperSimpleBlobStore.Common
{
    public class ConfigurationProvider
    {
        public static ApplicationEnvironment EnvironmentType
        {
            get
            {
                return (ApplicationEnvironment)Enum.Parse(typeof(ApplicationEnvironment), ConfigurationManager.AppSettings["EnvironmentType"]);
            }
        }

        public static string ApplicationName
        {
            get
            {
                return "SuperSimpleBlobStorage";
            }
        }

        public static string TokenHeaderName
        {
            get
            {
                return "X-Storage-Token";
            }
        }

        public static string ServicePort
        {
            get
            {
                return ConfigurationManager.AppSettings["ServicePort"];
            }
        }

        public static string UserAccountsDatabaseConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["UserAccounts"].ConnectionString;
            }
        }

        public static string BlobStoreDatabaseConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BlobStore"].ConnectionString;
            }
        }

        public static string LoggingConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Logging"].ConnectionString;
            }
        }


    }
}
