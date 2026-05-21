using System;
using System.Runtime.InteropServices;

namespace Client.Platform.FNA
{
    public static class NativeFallbackSDL
    {
        private const string LibSDL2 = "libSDL2-2.0.so.0";
        private const string LibFAudio = "libFAudio.so.0";

        public static bool IsFallbackAvailable { get; private set; }

        static NativeFallbackSDL()
        {
            try
            {
                // Test binding linkage
                var version = GetSDLVersion();
                IsFallbackAvailable = version != IntPtr.Zero;
                if (IsFallbackAvailable)
                {
                    Console.WriteLine("Antigravity Fallback Protocol: SDL2 native linkage verified successfully!");
                }
            }
            catch
            {
                IsFallbackAvailable = false;
                Console.WriteLine("Antigravity Fallback Protocol: Native SDL2 libraries not found, continuing with default FNA bindings.");
            }
        }

        #region SDL2 Direct P/Invokes
        [DllImport(LibSDL2, EntryPoint = "SDL_GetVersion", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_GetVersion(out SDL_Version version);

        [DllImport(LibSDL2, EntryPoint = "SDL_GetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetWindowTitle(IntPtr window);

        [DllImport(LibSDL2, EntryPoint = "SDL_SetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowTitle(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string title);

        [DllImport(LibSDL2, EntryPoint = "SDL_GetMouseState", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_GetMouseState(out int x, out int y);

        [DllImport(LibSDL2, EntryPoint = "SDL_GetKeyboardState", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardState(out int numkeys);

        [DllImport(LibSDL2, EntryPoint = "SDL_ShowSimpleMessageBox", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_ShowSimpleMessageBox(uint flags, [MarshalAs(UnmanagedType.LPStr)] string title, [MarshalAs(UnmanagedType.LPStr)] string message, IntPtr window);
        #endregion

        #region SDL Structural Types
        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Version
        {
            public byte major;
            public byte minor;
            public byte patch;
        }
        #endregion

        public static IntPtr GetSDLVersion()
        {
            try
            {
                SDL_GetVersion(out var version);
                var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<SDL_Version>());
                Marshal.StructureToPtr(version, ptr, false);
                return ptr;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        public static void ShowNativeError(string title, string message)
        {
            try
            {
                // SDL_MESSAGEBOX_ERROR = 0x00000010
                SDL_ShowSimpleMessageBox(0x00000010, title, message, IntPtr.Zero);
            }
            catch
            {
                // Fallback to standard Console error
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{title}] ERROR: {message}");
                Console.ResetColor();
            }
        }
    }
}
