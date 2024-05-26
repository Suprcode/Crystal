using System.Diagnostics;

namespace Shared.Helpers {
    public static class FileIO {
        public static void OpenScript(string scriptPath, bool useShellExecutable) {
            Process p = new() {
                StartInfo = new ProcessStartInfo(scriptPath) {
                    UseShellExecute = useShellExecutable
                }
            };

            p.Start();
        }
    }
}
