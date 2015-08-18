using System;
using System.Windows.Forms;

namespace AutoPatcher
{
    public static class Settings
    {
        private static readonly InIReader Reader = new InIReader(@".\Mir2Config.ini");

        public static string Host = @""; //ftp://212.67.209.184
        public static string PatchFileName = @"PList.gz";

        public static bool NeedLogin  =false;
        public static string Login = string.Empty;
        public static string Password = string.Empty;
        public static bool AllowCleanUp = true;

        public static string Client;
        public static bool AutoStart;

        public static void Load()
        {

            Host = Reader.ReadString("AutoPatcher", "Host", Host);
            PatchFileName = Reader.ReadString("AutoPatcher", "PatchFile", PatchFileName);

            NeedLogin = Reader.ReadBoolean("AutoPatcher", "NeedLogin", NeedLogin);
            Login = Reader.ReadString("AutoPatcher", "Login", Login);
            Password = Reader.ReadString("AutoPatcher", "Password", Password);

            AllowCleanUp = Reader.ReadBoolean("AutoPatcher", "AllowCleanUp", AllowCleanUp);
            AutoStart = Reader.ReadBoolean("AutoPatcher", "AutoStart", AutoStart);

            Client = Application.StartupPath + "\\";
            if (!Host.EndsWith("/")) Host += "/";
            if (Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) Host = Host.Insert(0, "http://");
        }
    }
}
