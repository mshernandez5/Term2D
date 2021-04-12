using System;

namespace term2d
{
    /// <summary>
    ///     An interface allowing classes to receive
    ///     console key events detected by the console
    ///     input thread.
    /// </summary>
    interface KeyInputListener
    {
        /// <summary>
        ///     The OnKeyEvent() method will be called
        ///     asynchrounously from the game loop any time
        ///     the user enters a key into the console.
        /// </summary>
        /// <param name="keyInfo">
        ///     A ConsoleKeyInfo object detailing the
        ///     console key event.
        /// </param>
        void OnKeyEvent(ConsoleKeyInfo keyInfo);
    }
}