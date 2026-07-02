using Newtonsoft.Json;

namespace ExcelDataExport
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var main = new Main();
            var curExePath = Path.GetDirectoryName(Application.ExecutablePath);
            var jsonConfigFile = Path.Combine(curExePath, "LubanDataManagerConfig.Json");
            if (!File.Exists(jsonConfigFile))
            {
                JsonConfig.ConfigInstance = new JsonConfig(jsonConfigFile);
            }
            else
            {
                try
                {
                    var str = File.ReadAllText(jsonConfigFile);
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        var deserialized = JsonConvert.DeserializeObject<JsonConfig>(str);
                        if (deserialized != null)
                        {
                            JsonConfig.ConfigInstance = deserialized;
                            JsonConfig.ConfigInstance.configPath = jsonConfigFile;
                            JsonConfig.ConfigInstance.EnsureDefaults(); // 旧配置文件的空路径→插件默认值
                        }
                        else
                        {
                            JsonConfig.ConfigInstance = new JsonConfig(jsonConfigFile);
                        }
                    }
                    else
                    {
                        JsonConfig.ConfigInstance = new JsonConfig(jsonConfigFile);
                    }
                }
                catch
                {
                    JsonConfig.ConfigInstance = new JsonConfig(jsonConfigFile);
                }
            }
            main.RefreshSetting();
            Application.Run(main);
        }
    }
}