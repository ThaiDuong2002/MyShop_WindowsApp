using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace MyShopProject.Utilities
{
    public class SecurityConfig
    {
        public static byte[] GenerateEntropy()
        {
            var entropy = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }
            return entropy;
        }
        public static string EncryptString(string text, byte[] entropy)
        {
            var textInBytes = Encoding.UTF8.GetBytes(text);
            var cypherText = ProtectedData.Protect(textInBytes, entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(cypherText);
        }

        public static string DecryptString(string text, byte[] entropy)
        {
            var textInBytes = Convert.FromBase64String(text);
            var plainText = ProtectedData.Unprotect(textInBytes, entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plainText);
        }

        public static void saveToConfig(string username, string password, string servername, string databasename, string entropy, bool IsRemember)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["UserName"].Value = username;
                config.AppSettings.Settings["Password"].Value = password;
                config.AppSettings.Settings["ServerName"].Value = servername;
                config.AppSettings.Settings["DatabaseName"].Value = databasename;
                config.AppSettings.Settings["Entropy"].Value = entropy;
                config.AppSettings.Settings["IsRemember"].Value = IsRemember.ToString();
                config.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static string getValueFromConfig(string key) => ConfigurationManager.AppSettings[key]!;

        public static void removeAllConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["UserName"].Value = "";
            config.AppSettings.Settings["Password"].Value = "";
            config.AppSettings.Settings["ServerName"].Value = "";
            config.AppSettings.Settings["DatabaseName"].Value = "";
            config.AppSettings.Settings["Entropy"].Value = "";
            config.AppSettings.Settings["IsRemember"].Value = "";
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
