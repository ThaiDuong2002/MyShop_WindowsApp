using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject
{
    internal class Global
    {
      
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static void SaveScreen(string screenName)
        {
            Global.config.AppSettings.Settings["Screen"].Value = screenName;
            Global.config.Save(ConfigurationSaveMode.Full);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
