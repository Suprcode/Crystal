using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoPatcherAdmin
{
    public static class Settings
    {
        private static readonly string JsonFileName = Path.Combine(AppContext.BaseDirectory, "PatchAdmin.json");
        private static readonly string LegacyIniFileName = Path.Combine(AppContext.BaseDirectory, "PatchAdmin.ini");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public static string Client = @"S:\Patch\";
        public static string Host = @"ftp://127.0.0.1/";
        public static string Login = string.Empty;
        public static string Password = string.Empty;
        public static string Protocol = "Ftp";
        public static int Port = 0;

        public static bool AllowCleanUp = true;
        public static bool CompressFiles = false;

        public static void Load()
        {
            if (File.Exists(JsonFileName))
            {
                LoadJson();
                return;
            }

            if (File.Exists(LegacyIniFileName))
            {
                ImportLegacyIni();
                Save();
                return;
            }

            Save();
        }

        public static void Save()
        {
            var data = new AutoPatcherSettingsData
            {
                Client = Client,
                Host = Host,
                Login = Login,
                EncryptedPassword = EncryptPassword(Password),
                Protocol = Protocol,
                Port = Port,
                AllowCleanUp = AllowCleanUp,
                CompressFiles = CompressFiles,
            };

            File.WriteAllText(JsonFileName, JsonSerializer.Serialize(data, JsonOptions));
        }

        private static string EncryptPassword(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(encrypted);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string DecryptPassword(string encryptedBase64)
        {
            if (string.IsNullOrEmpty(encryptedBase64)) return string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(encryptedBase64);
                byte[] decrypted = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void LoadJson()
        {
            try
            {
                string json = File.ReadAllText(JsonFileName);
                var data = JsonSerializer.Deserialize<AutoPatcherSettingsData>(json, JsonOptions);
                if (data == null) return;

                Client = data.Client ?? Client;
                Host = data.Host ?? Host;
                Login = data.Login ?? Login;
                Protocol = data.Protocol ?? Protocol;
                Port = data.Port;
                AllowCleanUp = data.AllowCleanUp;
                CompressFiles = data.CompressFiles;
                Password = !string.IsNullOrEmpty(data.EncryptedPassword)
                    ? DecryptPassword(data.EncryptedPassword)
                    : data.Password ?? string.Empty;
            }
            catch
            {
                Save();
            }
        }

        private static void ImportLegacyIni()
        {
            Dictionary<string, string> values = ReadLegacyIniSection("AutoPatcher");

            Client = ReadString(values, "Client", Client);
            Host = ReadString(values, "Host", Host);
            Login = ReadString(values, "Login", Login);
            Password = ReadString(values, "Password", Password);
            Protocol = ReadString(values, "Protocol", Protocol);
            Port = ReadInt(values, "Port", Port);

            AllowCleanUp = ReadBool(values, "AllowCleanUp", AllowCleanUp);
            CompressFiles = ReadBool(values, "CompressFiles", CompressFiles);
        }

        private static Dictionary<string, string> ReadLegacyIniSection(string section)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(LegacyIniFileName)) return values;

            bool inSection = false;
            foreach (string rawLine in File.ReadAllLines(LegacyIniFileName))
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("#")) continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    inSection = string.Equals(line.Trim('[', ']'), section, StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                if (!inSection) continue;

                int separator = line.IndexOf('=');
                if (separator <= 0) continue;

                string key = line.Substring(0, separator).Trim();
                string value = line.Substring(separator + 1).Trim();
                values[key] = value;
            }

            return values;
        }

        private static string ReadString(Dictionary<string, string> values, string key, string fallback)
        {
            return values.TryGetValue(key, out string? value) && !string.IsNullOrWhiteSpace(value)
                ? value
                : fallback;
        }

        private static int ReadInt(Dictionary<string, string> values, string key, int fallback)
        {
            return values.TryGetValue(key, out string? value) && int.TryParse(value, out int result)
                ? result
                : fallback;
        }

        private static bool ReadBool(Dictionary<string, string> values, string key, bool fallback)
        {
            return values.TryGetValue(key, out string? value) && bool.TryParse(value, out bool result)
                ? result
                : fallback;
        }

        private sealed class AutoPatcherSettingsData
        {
            public string? Client { get; set; }
            public string? Host { get; set; }
            public string? Login { get; set; }
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? Password { get; set; } // legacy plaintext — read only for migration
            public string? EncryptedPassword { get; set; }
            public string? Protocol { get; set; }
            public int Port { get; set; }
            public bool AllowCleanUp { get; set; } = true;
            public bool CompressFiles { get; set; }
        }
    }
}
