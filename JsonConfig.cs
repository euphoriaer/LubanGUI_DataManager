using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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


        /// <summary>
        /// 从 exe 目录解析相对路径为绝对路径
        /// </summary>
        private static string ResolvePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return "";
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.GetFullPath(Path.Combine(baseDir, relativePath));
        }

        public string LubanPathRelative { get; set; } = "plugin/Luban";
        [JsonIgnore]
        public string LubanPath => ResolvePath(LubanPathRelative);

        public string LubanConfigPathRelative { get; set; } = "plugin/excel_data/luban_config.json";
        [JsonIgnore]
        public string LubanConfigPath => ResolvePath(LubanConfigPathRelative);

        public string ProtoBufPathRelative { get; set; } = "plugin/Luban/protoc.exe";
        [JsonIgnore]
        public string ProtoBufPath => ResolvePath(ProtoBufPathRelative);

        public string DataPathRelative { get; set; } = "plugin/excel_data/output_data";
        [JsonIgnore]
        public string DataPath => ResolvePath(DataPathRelative);

        public string ScriptsPathRelative { get; set; } = "plugin/proto_cs";
        [JsonIgnore]
        public string ScriptsPath => ResolvePath(ScriptsPathRelative);

        public string ExcelsPathRelative { get; set; } = "plugin/excel_data/Datas";
        [JsonIgnore]
        public string ExcelsPath => ResolvePath(ExcelsPathRelative);


        public bool json_data { get; set; }

        public bool json_cs { get; set; }

        public bool cs_bin { get; set; }

        public bool bin_cs { get; set; }

        public bool protobuf_bin { get; set; }
        public bool protobuf_cs { get; set; }

        /// <summary>导出时每种格式在目标目录下建立子文件夹（默认勾选）</summary>
        public bool use_subfolders { get; set; } = true;


        /// <summary>
        /// 将空字符串的路径恢复为预设默认值（防止旧配置文件覆盖新默认值）
        /// </summary>
        public void EnsureDefaults()
        {
            if (string.IsNullOrEmpty(LubanPathRelative)) LubanPathRelative = "plugin/Luban";
            if (string.IsNullOrEmpty(LubanConfigPathRelative)) LubanConfigPathRelative = "plugin/excel_data/luban_config.json";
            if (string.IsNullOrEmpty(ProtoBufPathRelative)) ProtoBufPathRelative = "plugin/Luban/protoc.exe";
            if (string.IsNullOrEmpty(DataPathRelative)) DataPathRelative = "plugin/excel_data/output_data";
            if (string.IsNullOrEmpty(ScriptsPathRelative)) ScriptsPathRelative = "plugin/proto_cs";
            if (string.IsNullOrEmpty(ExcelsPathRelative)) ExcelsPathRelative = "plugin/excel_data/Datas";
        }


        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(configPath))
            {
                return;
            }

            try
            {
                var dir = Path.GetDirectoryName(configPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var fileStr = JsonConvert.SerializeObject(ConfigInstance, Formatting.Indented);
                File.WriteAllText(configPath, fileStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
