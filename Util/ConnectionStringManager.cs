using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Util
{
    public class ConnectionStringManager
    {
        public static string GetConnectionString()
        {
            string returnValue = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DBConnectionString"];

            if (settings != null) returnValue = settings.ConnectionString;

            return returnValue;
        }
    }
}