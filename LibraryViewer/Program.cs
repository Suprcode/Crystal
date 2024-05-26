namespace CMirLibraryViewer {
    internal static class Program {
        public static bool LoadFailed = false;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LMain());
        }
    }
}
