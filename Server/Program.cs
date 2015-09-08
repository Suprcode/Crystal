using System;
using System.Windows.Forms;

namespace Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
                Packet.IsServer = true;

                Settings.Load();
                //Resource.Load();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SMain());

                Settings.Save();
        }
    }
}
