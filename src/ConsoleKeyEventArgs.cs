using System;

namespace Term2D
{
    /// <summary>
    ///     Stores information for a new console key event.
    /// </summary>
    public class ConsoleKeyEventArgs : EventArgs
    {
        /// <summary>
        ///     Information about the console input
        ///     which triggered the event.
        /// </summary>
        public ConsoleKeyInfo KeyInfo {get; private set;}

        /// <summary>
        ///     Initialize arguments for a new event.
        /// </summary>
        public ConsoleKeyEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }
}