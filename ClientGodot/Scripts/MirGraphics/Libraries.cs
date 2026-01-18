using System.Threading.Tasks;
using Godot;

namespace ClientGodot.Scripts.MirGraphics
{
    public static class Libraries
    {
        public static bool Loaded;
        public static MLibrary ChrSel, Prguse, Prguse2, Prguse3, UI_32bit, Items, Title;

        // Add more as needed

        public static void Load()
        {
            // Initialize libraries.
            // In a real scenario, this should be async or threaded to avoid freezing the UI.
            Task.Run(() =>
            {
                // Ensure DataPath ends with slash
                string dataPath = Settings.UserDataPath;
                // Settings.UserDataPath defaults to "user://Data/"
                // But for development, we might want "res://Data/" or a local path.
                // Let's assume the user put files in a folder "Data" next to the executable or project.

                // For this specific task, if we want to run in Editor, "res://Data" is best.
                // If "res://Data" doesn't exist, we might check "."

                ChrSel = new MLibrary(dataPath + "ChrSel");
                Prguse = new MLibrary(dataPath + "Prguse");
                Prguse2 = new MLibrary(dataPath + "Prguse2");
                Prguse3 = new MLibrary(dataPath + "Prguse3");
                UI_32bit = new MLibrary(dataPath + "UI_32bit");
                Title = new MLibrary(dataPath + "Title");
                Items = new MLibrary(dataPath + "Items");

                ChrSel.Initialize();
                // Prguse.Initialize(); // Lazy load others

                Loaded = true;
                GD.Print("Libraries Loaded (Async).");
            });
        }
    }
}
