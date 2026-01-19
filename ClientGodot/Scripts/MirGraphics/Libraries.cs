using System.Threading.Tasks;
using Godot;

namespace ClientGodot.Scripts.MirGraphics
{
    public static class Libraries
    {
        public static bool Loaded;
        public static MLibrary ChrSel, Prguse, Prguse2, Prguse3, UI_32bit, Items, Title;

        // Map Libraries
        public static MLibrary[] MapLibs = new MLibrary[400];

        // Character Libraries
        public static MLibrary[] CArmours, CWeapons, CHair, CHumEffect;
        // Other
        public static MLibrary[] Monsters;
        public static MLibrary FloorItems;
        public static MLibrary NPCs;

        public static void Load()
        {
            Task.Run(() =>
            {
                string dataPath = Settings.UserDataPath;

                ChrSel = new MLibrary(dataPath + "ChrSel");
                Prguse = new MLibrary(dataPath + "Prguse");
                Prguse2 = new MLibrary(dataPath + "Prguse2");
                Prguse3 = new MLibrary(dataPath + "Prguse3");
                UI_32bit = new MLibrary(dataPath + "UI_32bit");
                Title = new MLibrary(dataPath + "Title");
                Items = new MLibrary(dataPath + "Items");
                FloorItems = new MLibrary(dataPath + "DNItems");
                NPCs = new MLibrary(dataPath + "NPCs");

                ChrSel.Initialize();

                // Init Character Libs
                // Note: Paths should ideally match Settings.cs definitions, but here we hardcode standard relative paths
                InitLibrary(ref CArmours, dataPath + "Data/CArmour/", "00");
                InitLibrary(ref CWeapons, dataPath + "Data/CWeapon/", "00");
                InitLibrary(ref CHair, dataPath + "Data/CHair/", "00");
                InitLibrary(ref CHumEffect, dataPath + "Data/CHumEffect/", "00");
                InitLibrary(ref Monsters, dataPath + "Data/Monster/", "000");

                // Init MapLibs (Wemade Mir2)
                MapLibs[0] = new MLibrary(dataPath + "Map/WemadeMir2/Tiles");
                MapLibs[1] = new MLibrary(dataPath + "Map/WemadeMir2/Smtiles");
                // Lazy init

                Loaded = true;
                GD.Print("Libraries Loaded (Async).");
            });
        }

        private static void InitLibrary(ref MLibrary[] library, string path, string suffix)
        {
             // Simple implementation: check 00..10?
             // In original, it scans directory. Godot sandbox limits directory scanning sometimes.
             // We'll allocate a fixed size for now or try to scan if possible.
             // Let's assume a reasonable count.
             library = new MLibrary[50];
             for (int i = 0; i < library.Length; i++)
             {
                 library[i] = new MLibrary(path + i.ToString(suffix));
             }
        }
    }
}
