# LubanDataManager

A visual data export management tool based on [Luban](https://github.com/focus-creative-games/luban), providing an intuitive Windows desktop interface for managing game configuration table exporting, viewing, and editing.

## Features

### 📤 One-Click Export
- **cs-bin** — C# code + binary data
- **bin-cs** — Binary data import + C# loading code
- **Protobuf3** — Binary protobuf data export
- **Protobuf3 → C#** — Auto code generation via protoc
- Selectable format combinations, configuration auto-persisted

### 📋 Excel File Management
- Auto-lists all `.xlsx` files in the data directory
- **Double-click** to open built-in editor
- **Right-click context menu**:
  - 📂 Open — Launch built-in editor
  - ✏️ Rename — Rename the Excel file
  - 🗑️ Delete — Delete the Excel file (with confirmation)
  - 📁 Open Folder — Reveal in Windows Explorer

### ✏️ Built-in Excel Editor
- View and edit all cells by double-clicking a file
- **Auto-save** on cell change (500ms debounce), no manual save needed
- Keyword search filter — `Ctrl+F` to focus, type to filter rows
- `Ctrl+S` to force save, `Esc` to clear filter
- Excel-style column headers (A, B, ..., AA, AB) and row numbers
- Auto-detects number/boolean/text types to preserve data format
- Auto-save on window close

### 🎨 UI
- Modern flat UI powered by **ReaLTaiizor** control library
- Settings tab organized into "Path Settings" and "Export Formats" groups
- `ListView` file list with clear blue selection highlighting
- Resizable window with adaptive layout

### 💾 Configuration
- All paths stored as **relative paths** for portability
- Default paths point to bundled `plugin/` directory, works out of the box
- Legacy config files auto-migrated with new default values

## Architecture

| Technology | Description |
|---|---|
| **Framework** | .NET 10 (net10.0-windows) |
| **UI** | Windows Forms + ReaLTaiizor 3.8 |
| **Serialization** | Newtonsoft.Json |
| **Excel I/O** | ClosedXML |
| **Dialogs** | WindowsAPICodePack (CommonOpenFileDialog) |

## Quick Start

```bash
git clone <repo_url>
cd lubangui
dotnet restore
dotnet run
```

Publish for distribution:
```bash
dotnet publish -c Release -o ./publish
```

The `plugin/` directory is automatically copied to the output.

## Usage

### Settings Tab

| Setting | Description | Default |
|---|---|---|
| Excel Path | Directory containing `.xlsx` files | `plugin/excel_data/Datas` |
| Luban Path | Directory containing `Luban.exe` | `plugin/Luban` |
| Luban Config Path | `luban_config.json` file | `plugin/excel_data/luban_config.json` |
| Data Output Path | Output directory for data files | `plugin/excel_data/output_data` |
| Code Output Path | Output directory for generated code | `plugin/proto_cs` |
| Protobuf Path | `protoc.exe` file | `plugin/Luban/protoc.exe` |
| Export Formats | Check desired format combinations | (none) |

### Export Tab

1. View all Excel files in the list
2. Select desired export formats in Settings
3. Click **Export All**
4. Wait for Luban to complete

### Editing Excel

1. **Double-click** or **Right-click → Open** a file
2. Edit cells directly
3. Changes are auto-saved (status shows `✓ Saved HH:mm:ss`)
4. Use the search box at top to filter rows
5. Close the window when done

## Dependencies

| Package | Version | Purpose |
|---|---|---|
| ReaLTaiizor | 3.8.1.5 | UI controls |
| Newtonsoft.Json | 13.0.4 | JSON config |
| ClosedXML | 0.104.2 | Excel read/write |
| WindowsAPICodePack | 8.0.14 | System dialogs |

## License

MIT License — see [LICENSE](LICENSE)
