using System;
using Godot;

namespace ClientGodot.Scripts
{
    public static class CMain
    {
        public static long Time => System.Environment.TickCount64;

        public static void SaveError(string message)
        {
            GD.PrintErr($"[Error] {message}");
            // TODO: Write to log file in user://
        }
    }
}
