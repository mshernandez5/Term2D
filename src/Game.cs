using System;

namespace Term2D
{
    /// <summary>
    ///     Games using the term2D framework should
    ///     extend this abstract class, allowing the
    ///     framework to communicate with your game
    ///     in a standardized way.
    /// </summary>
    public abstract class Game : KeyInputListener
    {
        /// <summary>
        ///     The Init() method will be called once
        ///     upon framework initialization.
        /// </summary>
        /// <param name="canvas">
        ///     A reference to the automatically created
        ///     Canvas object that will be rendered by default.
        /// </param>
        public abstract void Init(Canvas canvas);

        /// <summary>
        ///     The Update() method will be called continuously
        ///     by the framework as part of the core game loop.
        /// </summary>
        /// <param name="updateInfo">
        ///     A set of references and information received
        ///     by the framework including a reference to the
        ///     active Canvas, keyboard input information,
        ///     and time since the last update.
        /// </param>
        /// <returns>
        ///     true to continue running, false to end the game loop.
        /// </returns>
        public abstract bool Update(UpdateInfo updateInfo);

        /// <summary>
        ///     The OnKeyEvent() method will be called
        ///     asynchrounously from the game loop any time
        ///     the user enters a key into the console.
        /// </summary>
        /// <param name="keyInfo">
        ///     A ConsoleKeyInfo object detailing the
        ///     console key event.
        /// </param>
        public abstract void OnKeyEvent(ConsoleKeyInfo keyInfo);
    }
}