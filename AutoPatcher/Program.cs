using System;
using System.Windows.Forms;

namespace AutoPatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Settings.Load();

            if (args.Length > 0)
                Settings.AutoStart = args[0] == "Auto";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AMain());
        }
    }
}
