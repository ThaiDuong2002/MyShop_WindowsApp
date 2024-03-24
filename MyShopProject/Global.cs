using MyShopProject.UserControls;
using MyShopProject.View;
using System.Configuration;

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

        public static void GetActiveButton()
        {
            for (int i = 0; i < Dashboard.menuBTN.Children.Count; i++)
            {
                if (Dashboard.menuBTN.Children[i] is MenuButton)
                {
                    var select = Dashboard.menuBTN.Children[i] as MenuButton;
                    if (select.btn.IsFocused == true)
                        select.isActive = true;
                    else
                        select.isActive = false;
                }
            }

            for (int i = 0; i < Dashboard.subMenuBTN.Children.Count; i++)
            {
                if (Dashboard.subMenuBTN.Children[i] is MenuButton)
                {
                    var select_ = Dashboard.subMenuBTN.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }
        }
    }
}
