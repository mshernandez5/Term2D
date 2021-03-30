using System;

namespace term2d
{
    /// <summary>
    ///     A struct to pass framework information
    ///     to the game on each update.
    /// </summary>
    public struct UpdateInfo
    {
        UpdateInfo(Canvas activeCanvas, ConsoleKey lastInput, bool isUnreadInput, double deltaTime)
        {
            ActiveCanvas = activeCanvas;
            LastInput = lastInput;
            HasUnreadInput = isUnreadInput;
            DeltaTime = deltaTime;
        }

        /// <summary>
        ///     Reference To The Active Canvas
        /// </summary>
        public Canvas ActiveCanvas {get; set;}

        /// <summary>
        ///     The last key entered by the user.
        /// </summary>
        public ConsoleKey LastInput {get; set;}
        /// <summary>
        ///     Whether the input is new or has been previously read.
        /// </summary>
        public bool HasUnreadInput {get; set;}

        /// <summary>
        ///     Time since the last update, in seconds.
        /// </summary>
        public double DeltaTime {get; set;}
    }
}