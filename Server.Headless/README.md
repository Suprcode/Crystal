# Legend of Mir Crystal - Headless Server

This project is the modernized, headless version of the Legend of Mir Crystal Server, fully migrated to **.NET 10.0**. It is designed specifically to run natively and efficiently on Linux environments without any dependencies on Windows-specific UI frameworks (like Windows Forms) or image rendering libraries.

## 🌟 Features
- **True Headless Design:** Fully decoupled from UI forms; runs directly as a console application.
- **High-Precision Tick Loop:** Utilizes a custom Spin-Loop throttling mechanism for hyper-accurate 1ms tick alignment without yielding to the Linux OS scheduler, guaranteeing zero timing dilation.
- **Native Linux Path Resolution:** Automatically normalizes directory separators and enforces consistent lowercasing for map loaders to seamlessly resolve paths on case-sensitive Linux filesystems.
- **Lightweight Serialization:** Features a hand-rolled, type-safe binary serialization protocol for the buff system, fully replacing the obsolete and insecure `BinaryFormatter`.

---

## 🛠️ Prerequisites

To run this server natively, your environment must have:
- **OS:** Linux (Ubuntu/Debian, CentOS, AlmaLinux, etc.) or Windows (cross-platform compatible).
- **Runtime:** [.NET 10.0 SDK or Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

---

## 🚀 Deployment & Execution

### 1. Building the Server
To compile the headless server from the source code, navigate to the root solution directory and run:
```bash
dotnet build Server.Headless/Server.Headless.csproj -c Release
```
This will compile the `Server.Headless`, `Server.Library`, and `Shared` projects and output the executable to `Server.Headless/bin/Release/net10.0/`.

### 2. Environment Setup
The server requires specific data folders to operate. Place your built executable alongside the following core directories (often found in the release `Jev` package):
- `Server.MirDB` (Your main database file)
- `Configs/` (Server settings and configurations)
- `Envir/` (Drops, NPCs, Quests, Routes, and Scripts)
- `Maps/` (Game map `.map` files)

*Note: If you are migrating maps from a Windows server, it is highly recommended to batch-rename all your `.map` files to lowercase to ensure native case-sensitive resolution.*

### 3. Running the Server
You can launch the server directly using the `.NET` CLI from the directory containing your environment files:
```bash
dotnet run --project path/to/Server.Headless.csproj -c Release
```
Or run the published executable directly:
```bash
./Server.Headless
```

### 4. Graceful Shutdown
To stop the server safely, press `Ctrl + C` in the terminal or type `exit` in the console. The headless host intercepts the shutdown request to halt the game loop, securely save the database state, and commit all configurations before fully exiting.

---

## 🎮 CLI Interactive Console & `account` Command

The headless server starts an interactive console supporting auto-completion (tab-completion) and administrative commands.

### Interactive Commands
- `help` / `?`: Show the list of available commands.
- `exit`: Gracefully stop the server, saving the database state and committing configurations.
- `account`: View and edit user accounts and their nested attributes (characters, items, stats) in an OpenWrt `uci`-like format.

#### Account Path Notation & Subcommands
The `account` command works by resolving a dot-notation path, for example: `AccountID.Characters[Index].Inventory[Index].Field` or `AccountID.CharacterName.Inventory[Index].StatName`.

Available subcommands:
- **`account show`**: List all accounts, or recursively display nested attributes at a resolved path.
  ```bash
  account show asdf
  account show asdf.Honoka
  account show asdf.Honoka.Inventory
  ```
  *Note on array truncation:* When showing an object, nested arrays or lists containing more than 5 elements are truncated (omitting the remainder) to avoid terminal flooding. However, if the queried target *is* an array/list itself (e.g. `account show asdf.Honoka.Inventory`), it will display all elements without limits.
  *Note on privacy:* Sensitive fields like `Password` and `StoragePassword` are automatically displayed as `[Protected]` to prevent ANSI-corrupted/garbled text output.

- **`account get <path>`**: Get the value of a specific attribute.
  ```bash
  account get asdf.Gold
  account get asdf.Honoka.HP
  ```

- **`account set <path>=<value>`**: Modify the value of a specific attribute.
  ```bash
  account set asdf.Gold=1000
  account set asdf.Honoka.Level=15
  account set asdf.Honoka.Inventory[0].Luck=9
  ```
  *Shortcuts & Autocomplete:*
  - **Dynamic Tab Completion**: Pressing Tab on any path argument (e.g. `asdf.`) dynamically inspects the C# object hierarchy using reflection and offers completions for all nested fields, properties, character names, and `Stat` shortcuts.
  - If a segment is not found on `AccountInfo`, it will search the `Characters` list for a character matching the name (e.g., `asdf.Honoka` instead of `asdf.Characters[0]`).
  - If a segment is not found on `UserItem`, it will search the `Stat` enum and direct the set operation to the item's `AddedStats` collection.
  - Updates to online players (e.g. leveling up, item stats modifications, gold/credits changes) are automatically synchronized in real-time.

---

## ⚙️ Configuration

### Game Settings (`Configs/Settings.ini`)
When the server runs for the first time, it will automatically generate or load `Settings.ini` inside the `Configs/` directory. You can edit this file to adjust ports, experience rates, drop rates, and other environmental properties.

### Logging (`log4net.config`)
Console output and log files are managed by `log4net`. The logging configuration is located in `log4net.config` at the root of the executable directory. 
- Logs are automatically saved to `Logs/Server/`, `Logs/Chat/`, `Logs/Player/`, etc.
- By default, Linux-native forward slashes are utilized in the log paths.

---

## 🔧 Troubleshooting

- **Script Not Found Errors:** If the server console reports `Script Not Found` or `INSERT Script Not Found`, ensure that all custom scripts referenced in your `Envir/` directory exist and that their internal references (`#INCLUDE`, `#INSERT`) correctly match your file structure.
- **Failed to Load Map:** Ensure your map files are properly placed in the `Maps/` directory and are named in strictly lowercase (e.g., `d011.map` instead of `D011.map`) to satisfy Linux filesystem requirements.
- **Address Already in Use:** If the server fails to bind the network socket, ensure no other instance of the server is running in the background and that your configured port (default TCP `7000`) is free.
