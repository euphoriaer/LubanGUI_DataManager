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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var main = new Main();
            var curExePath = Path.GetDirectoryName(Application.ExecutablePath);
            var jsonConfigFile = Path.Combine(curExePath, "LubanDataManagerConfig.Json");
            if (!File.Exists(jsonConfigFile))
            {
                MessageBox.Show("请配置导出路径");
                JsonConfig.ConfigInstance = new JsonConfig(jsonConfigFile);
            }
            else
            {

                var str = File.ReadAllText(jsonConfigFile);
                JsonConfig.ConfigInstance = JsonConvert.DeserializeObject<JsonConfig>(str);
                JsonConfig.ConfigInstance.configPath = jsonConfigFile;

            }
            main.RefreshSetting();
            Application.Run(main);
        }
    }
}