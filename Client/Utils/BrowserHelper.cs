using System.Diagnostics;
using Microsoft.Win32;

namespace Client.Utils {
    public class BrowserHelper {
        private static void OpenChrometBrowser(string url) {
            try {
                string openKey = @"SOFTWARE\Wow6432Node\Google\Chrome";
                if(IntPtr.Size == 4) {
                    openKey = @"SOFTWARE\Google\Chrome";
                }

                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);

                if(appPath != null) {
                    Process result = Process.Start("chrome.exe", url);
                    if(result == null) {
                        OpenIetBrowser(url);
                    }
                } else {
                    OpenDefaultBrowser(url);
                }
            } catch {
                OpenDefaultBrowser(url);
            }
        }

        private static void OpenIetBrowser(string url) {
            try {
                Process.Start("iexplore.exe", url);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);

                try {
                    if(File.Exists(@"C:\Program Files\Internet Explorer\iexplore.exe")) {
                        ProcessStartInfo processStartInfo = new() {
                            FileName = @"C:\Program Files\Internet Explorer\iexplore.exe",
                            Arguments = url,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(processStartInfo);
                    } else {
                        if(File.Exists(@"C:\Program Files (x86)\Internet Explorer\iexplore.exe")) {
                            ProcessStartInfo processStartInfo = new() {
                                FileName = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe",
                                Arguments = url,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };
                            Process.Start(processStartInfo);
                        } else {
                            if(MessageBox.Show(
                                   "Internet Explorer is not installed in the system. Do you want to download and install it?",
                                   null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes) {
                                OpenDefaultBrowser("http://windows.microsoft.com/zh-cn/internet-explorer/download-ie");
                            }
                        }
                    }
                } catch(Exception exception) {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        public static void OpenDefaultBrowser(string url) {
            try {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                if(key != null) {
                    string s = key.GetValue("").ToString();
                    int lastIndex = s.IndexOf(".exe", StringComparison.Ordinal);
                    string path = s.Substring(1, lastIndex + 3);
                    Process result = Process.Start(path, url);
                    if(result == null) {
                        Process result1 = Process.Start("explorer.exe", url);
                        if(result1 == null) {
                            Process.Start(url);
                        }
                    }
                } else {
                    Process result1 = Process.Start("explorer.exe", url);
                    if(result1 == null) {
                        Process.Start(url);
                    }
                }
            } catch {
                OpenIetBrowser(url);
            }
        }
    }
}
