using System;
using System.IO;
using FontStashSharp;

namespace Client.Platform.FNA
{
    public static class FNAFontManager
    {
        public static FontSystem FontSystem { get; private set; }

        static FNAFontManager()
        {
            FontSystem = new FontSystem();
            
            // Look for standard high-quality TrueType fonts on Linux
            string[] systemFontPaths = new[]
            {
                "/usr/share/fonts/google-droid-sans-fonts/DroidSans-Bold.ttf"
            };

            string resolvedFontPath = null;
            foreach (var path in systemFontPaths)
            {
                if (File.Exists(path))
                {
                    resolvedFontPath = path;
                    break;
                }
            }

            if (resolvedFontPath != null)
            {
                try
                {
                    FontSystem.AddFont(File.ReadAllBytes(resolvedFontPath));
                    Console.WriteLine($"[FNAFontManager] Successfully loaded TrueType font: {resolvedFontPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FNAFontManager] Error loading system font {resolvedFontPath}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("[FNAFontManager] Warning: No standard system TrueType fonts found! Dynamic text may fail to render.");
            }

            // Look for CJK (Chinese, Japanese, Korean) fallback fonts on Linux
            string[] chineseFontPaths = new[]
            {
                "/usr/share/fonts/fandol/FandolHei-Bold.otf"
            };

            foreach (var path in chineseFontPaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        FontSystem.AddFont(File.ReadAllBytes(path));
                        Console.WriteLine($"[FNAFontManager] Successfully loaded CJK fallback font: {path}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[FNAFontManager] Error loading CJK fallback font {path}: {ex.Message}");
                    }
                }
            }
        }

        public static SpriteFontBase GetFont(float size)
        {
            // System.Drawing.Font.Size is traditionally in Points (1/72 inch). 
            // FontStashSharp expects size in Pixels.
            // At standard 96 DPI, 1 Point = 96/72 = 1.333... Pixels.
            float pixelSize = size * (96f / 72f);
            int roundedSize = (int)Math.Max(1, Math.Round(pixelSize));
            return FontSystem.GetFont(roundedSize);
        }
    }
}
