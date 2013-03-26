using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save
{
    public class LocalStorage
    {
        public static void SaveRoaming(string key, string value)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =  Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[key] = value;
        }

        public static string LoadRoaming(string key)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey(key))
            {
                return roamingSettings.Values[key].ToString();
            }
            else
            {
                return null;
            }

        }
    }
}
