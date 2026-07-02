# LubanDataManager

基于 [Luban](https://github.com/focus-creative-games/luban) 的可视化数据导出管理工具，提供直观的 Windows 桌面界面来管理游戏配置表的导出、查看和编辑。

## 功能特性

### 📤 一键导出
- 支持 **Json** 数据导出（`data_json`）
- 支持 **json-cs**（C# 代码 + JSON 数据加载）
- 支持 **cs-bin**（C# 代码 + 二进制数据）
- 支持 **bin-cs**（二进制数据 + C# 加载代码）
- 支持 **protobuf3** 数据导出
- 支持 **protobuf3-cs**（Protobuf3 → C# 代码自动生成，调用 protoc）
- 格式可自由组合勾选，配置自动持久化

### 📋 Excel 文件管理
- 自动列出 Excel 数据目录下所有 `.xlsx` 文件
- **双击** 打开内置编辑器查看和修改
- **右键菜单**：
  - 📂 打开 — 启动内置编辑器
  - ✏️ 重命名 — 重命名 Excel 文件
  - 🗑️ 删除 — 删除 Excel 文件（带确认）
  - 📁 打开所在文件夹 — 在资源管理器中定位

### ✏️ 内置 Excel 编辑器
- 双击文件即可查看/编辑全部单元格
- **修改即保存**（500ms 防抖），无需手动点击保存
- 关键词搜索过滤 — `Ctrl+F` 聚焦搜索框，输入即过滤行
- 支持 `Ctrl+S` 立即保存、`Esc` 清除过滤
- Excel 风格列名（A, B, ..., AA, AB）和行号
- 自动识别数字/布尔/文本类型，避免数据格式丢失
- 关闭窗口自动保存

### 🎨 界面特性
- 基于 **ReaLTaiizor** 控件库的现代化扁平界面
- 设置页分「路径设置」和「导出格式」两大分组
- `ListView` 文件列表，蓝色选中高亮清晰可辨
- 支持窗体缩放，自适应布局

### 💾 配置管理
- 所有路径自动计算为**相对路径**存储，项目可移植
- 默认路径指向内置 `plugin/` 随包目录，开箱即用
- 旧版本配置文件自动补全新版默认路径

## 软件架构

```
LubanDataManager/
├── Main.cs              # 主窗口逻辑（UI事件、导出流程、右键菜单）
├── ExcelViewer.cs        # Excel 查看/编辑窗口（自动保存）
├── JsonConfig.cs         # JSON 配置读写（相对路径解析）
├── DialogTools.cs        # 文件/文件夹选择对话框封装
├── Program.cs            # 程序入口（配置加载与恢复）
├── plugin/               # 内置工具链
│   ├── Luban/            # Luban.exe + 全套 DLL + protoc.exe
│   ├── excel_data/       # Excel 数据文件 + luban_config.json
│   └── proto_cs/         # 生成的 proto C# 代码
└── ExcelDataExport.csproj
```

| 技术 | 说明 |
|---|---|
| **框架** | .NET 10 (net10.0-windows) |
| **UI** | Windows Forms + ReaLTaiizor 3.8 |
| **序列化** | Newtonsoft.Json |
| **Excel 操作** | ClosedXML |
| **对话框** | WindowsAPICodePack (CommonOpenFileDialog) |

## 安装与运行

### 方式一：编译运行

```bash
git clone <repo_url>
cd lubangui
dotnet restore
dotnet run
```

### 方式二：发布打包

```bash
dotnet publish -c Release -o ./publish
```

`plugin/` 目录会自动拷贝到输出目录。

## 使用说明

### 首次启动

1. 程序启动后自动进入「设置」页面
2. 各路径已有默认值（指向 `plugin/` 下的内置工具链）
3. 如有自定义需求，点击右侧按钮选择对应目录或文件

### 设置页面

| 配置项 | 说明 | 默认值 |
|---|---|---|
| Excel 路径 | 存放 `.xlsx` 配置表的目录 | `plugin/excel_data/Datas` |
| Luban 路径 | Luban.exe 所在目录 | `plugin/Luban` |
| Luban 配置路径 | `luban_config.json` 文件 | `plugin/excel_data/luban_config.json` |
| 数据导出路径 | 导出数据（bin/json）的存放目录 | `plugin/excel_data/output_data` |
| 代码导出路径 | 生成代码（cs/proto）的存放目录 | `plugin/proto_cs` |
| Protobuf 路径 | protoc.exe 文件 | `plugin/Luban/protoc.exe` |
| 导出格式 | 勾选需要的数据/代码格式 | 全部未勾选 |

### 导出页面

1. 在「导出」页面查看所有 Excel 文件
2. 勾选需要的导出格式
3. 点击 **导出全部** 按钮
4. 等待 Luban 处理完成

### 编辑 Excel

1. 在文件列表中**双击**或**右键 → 打开**
2. 在原单元格中直接修改
3. 修改自动保存（状态栏显示 `✓ 已保存 HH:mm:ss`）
4. 顶部搜索框输入关键词过滤行
5. 关闭窗口退出

## 开发

### 依赖 NuGet

| 包 | 版本 | 用途 |
|---|---|---|
| ReaLTaiizor | 3.8.1.5 | UI 控件库 |
| Newtonsoft.Json | 13.0.4 | JSON 配置 |
| ClosedXML | 0.104.2 | Excel 读写 |
| WindowsAPICodePack | 8.0.14 | 系统对话框 |
| FontAwesome.WPF | 4.7.0.9 | 图标（保留） |

### 项目结构

```bash
dotnet build          # Debug 编译
dotnet run            # 运行
dotnet publish -c Release  # 发布
```

## 开源协议

MIT License - 详见 [LICENSE](LICENSE) 文件
