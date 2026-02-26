using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelDataExport
{
    public class JsonConfig
    {
        public static JsonConfig ConfigInstance { get; set; }


        [JsonIgnore]
        public string configPath { get; set; }

        public JsonConfig(string jsonFilePath)
        {
            configPath = jsonFilePath;

        }


        public string LubanPathRelative { get; set; } = "";
        [JsonIgnore]
        public string LubanPath
        {
            get
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.GetFullPath(Path.Combine(baseDir, LubanPathRelative));
            }
        }
        public string LubanConfigPathRelative { get; set; } = "";
        [JsonIgnore]
        public string LubanConfigPath
        {
            get
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.GetFullPath(Path.Combine(baseDir, LubanConfigPathRelative));
            }
        }

        public string ProtoBufPathRelative { get; set; } = "";
        [JsonIgnore]
        public string ProtoBufPath
        {
            get
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.GetFullPath(Path.Combine(baseDir, ProtoBufPathRelative));
            }
        }


        [JsonIgnore]
        public string DataPath
        {
            get
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.GetFullPath(Path.Combine(baseDir, DataPathRelative));
            }
        }
        public string DataPathRelative { get; set; } = "";
        [JsonIgnore]
        public string ScriptsPath
        {
            get
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.GetFullPath(Path.Combine(baseDir, ScriptsPathRelative));
            }
        }
        public string ScriptsPathRelative { get; set; } = "";


        [JsonIgnore]
        public string ExcelsPath
        {
            get
            {
                string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.GetFullPath(Path.Combine(baseDir, ExcelsPathRelative));
            }
        }
        public string ExcelsPathRelative { get; set; } = "";


        public bool cs_bin { get; set; }

        public bool bin_cs { get; set; }

        public bool protobuf_bin { get; set; }
        public bool protobuf_cs { get; set; }


        public void SaveConfig()
        {
            var fileStr = JsonConvert.SerializeObject(ConfigInstance, Formatting.Indented);
            File.WriteAllText(configPath, fileStr);
        }
    }
}
