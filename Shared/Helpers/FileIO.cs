using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public static class FileIO
    {
        public static void OpenScript(string scriptPath, bool useShellExecutable)
        {
            Process p = new()
            {
                StartInfo = new ProcessStartInfo(scriptPath)
                {
                    UseShellExecute = useShellExecutable
                }
            };

            p.Start();
        }
    }
}
