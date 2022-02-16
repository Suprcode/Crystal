using System.IO;

namespace Map_Editor
{
  public static class Libraries
  {
    public static bool Loaded;
    public static int Count;
    public static int Progress;
    public const string LibPath = ".\\Data\\Map\\WemadeMir2\\";
    public const string ShandaMir2LibPath = ".\\Data\\Map\\ShandaMir2\\";
    public const string ObjectsPath = ".\\Data\\Objects\\";
    public static readonly MLibrary[] MapLibs = new MLibrary[400];
    public static readonly ListItem[] ListItems = new ListItem[400];

    static Libraries()
    {
      Libraries.MapLibs[0] = new MLibrary(".\\Data\\Map\\WemadeMir2\\Tiles");
      Libraries.ListItems[0] = new ListItem("Tiles", 0);
      Libraries.MapLibs[1] = new MLibrary(".\\Data\\Map\\WemadeMir2\\Smtiles");
      Libraries.ListItems[1] = new ListItem("Smtiles", 1);
      Libraries.MapLibs[2] = new MLibrary(".\\Data\\Map\\WemadeMir2\\Objects");
      Libraries.ListItems[2] = new ListItem("Objects", 2);
      for (int index = 2; index < 24; ++index)
      {
        if (File.Exists(".\\Data\\Map\\WemadeMir2\\Objects" + (object) index + ".lib"))
        {
          Libraries.MapLibs[index + 1] = new MLibrary(".\\Data\\Map\\WemadeMir2\\Objects" + (object) index);
          Libraries.ListItems[index + 1] = new ListItem("Objects" + (object) index, index + 1);
        }
      }
      Libraries.MapLibs[100] = new MLibrary(".\\Data\\Map\\ShandaMir2\\Tiles");
      Libraries.ListItems[100] = new ListItem("Tiles", 100);
      for (int index = 1; index < 10; ++index)
      {
        if (File.Exists(".\\Data\\Map\\ShandaMir2\\Tiles" + (object) (index + 1) + ".lib"))
        {
          Libraries.MapLibs[100 + index] = new MLibrary(".\\Data\\Map\\ShandaMir2\\Tiles" + (object) (index + 1));
          Libraries.ListItems[100 + index] = new ListItem("Tiles" + (object) (index + 1), 100 + index);
        }
      }
      Libraries.MapLibs[110] = new MLibrary(".\\Data\\Map\\ShandaMir2\\SmTiles");
      Libraries.ListItems[110] = new ListItem("SmTiles", 110);
      for (int index = 1; index < 10; ++index)
      {
        if (File.Exists(".\\Data\\Map\\ShandaMir2\\SmTiles" + (object) (index + 1) + ".lib"))
        {
          Libraries.MapLibs[110 + index] = new MLibrary(".\\Data\\Map\\ShandaMir2\\SmTiles" + (object) (index + 1));
          Libraries.ListItems[110 + index] = new ListItem("SmTiles" + (object) (index + 1), 110 + index);
        }
      }
      Libraries.MapLibs[120] = new MLibrary(".\\Data\\Map\\ShandaMir2\\Objects");
      Libraries.ListItems[120] = new ListItem("Objects", 120);
      for (int index = 1; index < 31; ++index)
      {
        if (File.Exists(".\\Data\\Map\\ShandaMir2\\Objects" + (object) (index + 1) + ".lib"))
        {
          Libraries.MapLibs[120 + index] = new MLibrary(".\\Data\\Map\\ShandaMir2\\Objects" + (object) (index + 1));
          Libraries.ListItems[120 + index] = new ListItem("Objects" + (object) (index + 1), 120 + index);
        }
      }
      Libraries.MapLibs[190] = new MLibrary(".\\Data\\Map\\ShandaMir2\\AniTiles1");
      Libraries.ListItems[190] = new ListItem("AniTiles1", 190);
      string[] strArray1 = new string[5]
      {
        "",
        "wood\\",
        "sand\\",
        "snow\\",
        "forest\\"
      };
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Tilesc.lib"))
        {
          Libraries.MapLibs[200 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Tilesc");
          Libraries.ListItems[200 + index * 15] = new ListItem(strArray1[index] + "Tilesc", 200 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Tiles30c.lib"))
        {
          Libraries.MapLibs[201 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Tiles30c");
          Libraries.ListItems[201 + index * 15] = new ListItem(strArray1[index] + "Tiles30c", 201 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Tiles5c.lib"))
        {
          Libraries.MapLibs[202 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Tiles5c");
          Libraries.ListItems[202 + index * 15] = new ListItem(strArray1[index] + "Tiles5c", 202 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Smtilesc.lib"))
        {
          Libraries.MapLibs[203 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Smtilesc");
          Libraries.ListItems[203 + index * 15] = new ListItem(strArray1[index] + "Smtilesc", 203 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Housesc.lib"))
        {
          Libraries.MapLibs[204 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Housesc");
          Libraries.ListItems[204 + index * 15] = new ListItem(strArray1[index] + "Housesc", 204 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Cliffsc.lib"))
        {
          Libraries.MapLibs[205 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Cliffsc");
          Libraries.ListItems[205 + index * 15] = new ListItem(strArray1[index] + "Cliffsc", 205 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Dungeonsc.lib"))
        {
          Libraries.MapLibs[206 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Dungeonsc");
          Libraries.ListItems[206 + index * 15] = new ListItem(strArray1[index] + "Dungeonsc", 206 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Innersc.lib"))
        {
          Libraries.MapLibs[207 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Innersc");
          Libraries.ListItems[207 + index * 15] = new ListItem(strArray1[index] + "Innersc", 207 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Furnituresc.lib"))
        {
          Libraries.MapLibs[208 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Furnituresc");
          Libraries.ListItems[208 + index * 15] = new ListItem(strArray1[index] + "Furnituresc", 208 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Wallsc.lib"))
        {
          Libraries.MapLibs[209 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Wallsc");
          Libraries.ListItems[209 + index * 15] = new ListItem(strArray1[index] + "Wallsc", 209 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "smObjectsc.lib"))
        {
          Libraries.MapLibs[210 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "smObjectsc");
          Libraries.ListItems[210 + index * 15] = new ListItem(strArray1[index] + "smObjectsc", 210 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Animationsc.lib"))
        {
          Libraries.MapLibs[211 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Animationsc");
          Libraries.ListItems[211 + index * 15] = new ListItem(strArray1[index] + "Animationsc", 211 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Object1c.lib"))
        {
          Libraries.MapLibs[212 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Object1c");
          Libraries.ListItems[212 + index * 15] = new ListItem(strArray1[index] + "Object1c", 212 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Object2c.lib"))
        {
          Libraries.MapLibs[213 + index * 15] = new MLibrary(".\\Data\\Map\\WemadeMir3\\" + strArray1[index] + "Object2c");
          Libraries.ListItems[213 + index * 15] = new ListItem(strArray1[index] + "Object2c", 213 + index * 15);
        }
      }
      string[] strArray2 = new string[5]
      {
        "",
        "wood",
        "sand",
        "snow",
        "forest"
      };
      for (int index = 0; index < strArray2.Length; ++index)
      {
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Tilesc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[300 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Tilesc" + strArray2[index]);
          Libraries.ListItems[300 + index * 15] = new ListItem("Tilesc" + strArray2[index], 300 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Tiles30c" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[301 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Tiles30c" + strArray2[index]);
          Libraries.ListItems[301 + index * 15] = new ListItem("Tiles30c" + strArray2[index], 301 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Tiles5c" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[302 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Tiles5c" + strArray2[index]);
          Libraries.ListItems[302 + index * 15] = new ListItem("Tiles5c" + strArray2[index], 302 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Smtilesc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[303 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Smtilesc" + strArray2[index]);
          Libraries.ListItems[303 + index * 15] = new ListItem("Smtilesc" + strArray2[index], 303 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Housesc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[304 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Housesc" + strArray2[index]);
          Libraries.ListItems[304 + index * 15] = new ListItem("Housesc" + strArray2[index], 304 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Cliffsc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[305 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Cliffsc" + strArray2[index]);
          Libraries.ListItems[305 + index * 15] = new ListItem("Cliffsc" + strArray2[index], 305 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Dungeonsc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[306 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Dungeonsc" + strArray2[index]);
          Libraries.ListItems[306 + index * 15] = new ListItem("Dungeonsc" + strArray2[index], 306 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Innersc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[307 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Innersc" + strArray2[index]);
          Libraries.ListItems[307 + index * 15] = new ListItem("Innersc" + strArray2[index], 307 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Furnituresc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[308 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Furnituresc" + strArray2[index]);
          Libraries.ListItems[308 + index * 15] = new ListItem("Furnituresc" + strArray2[index], 308 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Wallsc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[309 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Wallsc" + strArray2[index]);
          Libraries.ListItems[309 + index * 15] = new ListItem("Wallsc" + strArray2[index], 309 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\smObjectsc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[310 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\smObjectsc" + strArray2[index]);
          Libraries.ListItems[310 + index * 15] = new ListItem("smObjectsc" + strArray2[index], 310 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Animationsc" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[311 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Animationsc" + strArray2[index]);
          Libraries.ListItems[311 + index * 15] = new ListItem("Animationsc" + strArray2[index], 311 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Object1c" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[312 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Object1c" + strArray2[index]);
          Libraries.ListItems[312 + index * 15] = new ListItem("Object1c" + strArray2[index], 312 + index * 15);
        }
        if (File.Exists(".\\Data\\Map\\ShandaMir3\\Object2c" + strArray2[index] + ".lib"))
        {
          Libraries.MapLibs[313 + index * 15] = new MLibrary(".\\Data\\Map\\ShandaMir3\\Object2c" + strArray2[index]);
          Libraries.ListItems[313 + index * 15] = new ListItem("Object2c" + strArray2[index], 313 + index * 15);
        }
      }
    }

    public static void LoadGameLibraries()
    {
      Libraries.Count = Libraries.MapLibs.Length;
      for (int index = 0; index < Libraries.MapLibs.Length; ++index)
      {
        if (Libraries.MapLibs[index] == null)
          Libraries.MapLibs[index] = new MLibrary("");
        else
          Libraries.MapLibs[index].Initialize();
        ++Libraries.Progress;
      }
      Libraries.Loaded = true;
    }
  }
}
