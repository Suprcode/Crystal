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
            // Smart Path Detection
            string resData = ProjectSettings.GlobalizePath("res://Data/");
            string appData = System.AppDomain.CurrentDomain.BaseDirectory + "Data/";
            string userData = ProjectSettings.GlobalizePath("user://Data/");

            GD.Print($"Checking Data Path [Res]: {resData}");
            GD.Print($"Checking Data Path [App]: {appData}");

            if (System.IO.Directory.Exists(resData))
            {
                UserDataPath = resData;
                GD.Print($"Selected: Resource Path");
            }
            else if (System.IO.Directory.Exists(appData))
            {
                UserDataPath = appData;
                GD.Print($"Selected: App Base Path");
            }
            else
            {
                UserDataPath = userData;
                GD.Print($"Selected: User Data Path (Default): {UserDataPath}");
            }

            // Normalize path end
            if (!UserDataPath.EndsWith("/") && !UserDataPath.EndsWith("\\"))
                UserDataPath += "/";

            // Fix separators?
            // Windows allows / usually, but consistent is better.
            UserDataPath = UserDataPath.Replace("\\", "/");
        }

		public static void Save()
		{
			 // TODO: Implement ConfigFile saving
		}
	}
}
