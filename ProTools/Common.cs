using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ProTools
{
    public class Common
    {
        public static string GetKeyValue(string keyname)
        {
            string value = "";
            if (ConfigurationManager.AppSettings.HasKeys())
            {
                if(ConfigurationManager.AppSettings.AllKeys.ToList().Contains(keyname))
                { 
                    value = ConfigurationManager.AppSettings[keyname];
                } 
            }
            return value;
        }
    }
}
