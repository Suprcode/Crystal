using System;
using Godot;

namespace ClientGodot.Scripts
{
    public static class Settings
    {
        public static string IPAddress = "127.0.0.1";
        public static int Port = 7000;
        public static int TimeOut = 5000;

        public static string AccountID = string.Empty;
        public static string Password = string.Empty;

        public static bool FullScreen = false;
        public static int ScreenWidth = 1024;
        public static int ScreenHeight = 768;

        // Path configurations adapted for Godot
        // "user://" refers to the user data directory in Godot
        public static string UserDataPath = "user://Data/";

        public static void Load()
        {
            // TODO: Implement ConfigFile (Ini) loading using Godot.ConfigFile
            // For now, we use defaults.
        }

        public static void Save()
        {
             // TODO: Implement ConfigFile saving
        }
    }
}
