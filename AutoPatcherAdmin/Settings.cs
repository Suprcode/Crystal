namespace AutoPatcherAdmin
{
    public static class Settings
    {
        private static readonly InIReader Reader = new InIReader(@".\PatchAdmin.ini");

        public static string Client = @"S:\Patch\";
        public static string Host = @"ftp://127.0.0.1/";
        public static string Login = string.Empty;
        public static string Password = string.Empty;
        public static string Protocol = "Ftp";

        public static bool AllowCleanUp = true;
        public static bool CompressFiles = false;

        public static void Load()
        {
            Client = Reader.ReadString("AutoPatcher", "Client", Client);
            Host = Reader.ReadString("AutoPatcher", "Host", Host);
            Login = Reader.ReadString("AutoPatcher", "Login", Login);
            Password = Reader.ReadString("AutoPatcher", "Password", Password);
            Protocol = Reader.ReadString("AutoPatcher", "Protocol", Protocol);

            AllowCleanUp = Reader.ReadBoolean("AutoPatcher", "AllowCleanUp", AllowCleanUp);
            CompressFiles = Reader.ReadBoolean("AutoPatcher", "CompressFiles", CompressFiles);
        }

        public static void Save()
        {
            Reader.Write("AutoPatcher", "Client", Client);
            Reader.Write("AutoPatcher", "Host", Host);
            Reader.Write("AutoPatcher", "Login", Login);
            Reader.Write("AutoPatcher", "Password", Password);
            Reader.Write("AutoPatcher", "Protocol", Protocol);

            Reader.Write("AutoPatcher", "AllowCleanUp", AllowCleanUp);
            Reader.Write("AutoPatcher", "CompressFiles", CompressFiles);
        }
    }
}
