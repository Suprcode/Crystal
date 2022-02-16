using System;
using System.Windows.Forms;

namespace Map_Editor
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Map_Editor.Main());
    }
  }
}
