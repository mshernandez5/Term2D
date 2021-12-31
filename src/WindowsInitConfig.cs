using System;
using System.Runtime.InteropServices;

namespace Term2D
{
    /// <summary>
    ///     A console configuration procedure specifically
    ///     for Windows systems that prevents problematic
    ///     default behaviors of cmd.exe through native functions.
    /// </summary>
    class WindowsInitConfig : InitConfig
    {
        /// <summary>
        ///     Identifies a bit corresponding to a console setting
        ///     that must be enabled to prevent auto-scrolling
        ///     issues that prevent proper rendering.
        /// </summary>
        const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;

        /// <summary>
        ///     Used to request a standard output handle
        ///     using the Win32 API.
        /// </summary>
        const int STD_OUTPUT_HANDLE = -11;

        /// <summary>
        ///     Used to request a standard output handle
        ///     using the Win32 API.
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern IntPtr GetStdHandle(int consoleHandle);

        /// <summary>
        ///     Writes an unsigned integer with bits
        ///     representing console configuration settings.
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        /// <summary>
        ///     Sets altered console configuration settings.
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public void initialize()
        {
            IntPtr consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            uint mode;
            if (!GetConsoleMode(consoleHandle, out mode))
            {
                Console.Out.WriteLine("ERROR GETTING CONSOLE MODE");
                Console.Out.WriteLine("The game may not render correctly on cmd.exe, press any key to continue...");
                Console.ReadKey(true);
            }
            mode &= ~ENABLE_WRAP_AT_EOL_OUTPUT;
            if (!SetConsoleMode(consoleHandle, mode))
            {
                Console.Out.WriteLine("ERROR SETTING CONSOLE MODE");
                Console.Out.WriteLine("The game may not render correctly on cmd.exe, press any key to continue...");
                Console.ReadKey(true);
            }
            Console.CursorVisible = false;
            Console.Clear();
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }
    }
}