using System;

namespace Term2D
{
    /// <summary>
    ///     An interface allowing classes to receive
    ///     console key events detected by the console
    ///     input thread.
    /// </summary>
    public interface KeyInputListener
    {
        /// <summary>
        ///     The OnKeyEvent() method will be called
        ///     asynchrounously from the render loop any time
        ///     the user enters a key into the console.
        /// </summary>
        /// <param name="sender">
        ///     The object which dispatched the event.
        /// </param>
        /// <param name="e">
        ///     A ConsoleKeyInfo object detailing the
        ///     console key event.
        /// </param>
        void OnKeyEvent(object sender, ConsoleKeyEventArgs e);
    }
}