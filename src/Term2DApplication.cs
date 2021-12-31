using System;

namespace Term2D
{
    /// <summary>
    ///     Applications using the term2D framework should
    ///     extend this abstract class.
    /// </summary>
    public abstract class Term2DApplication : KeyInputListener
    {
        /// <summary>
        ///     The Init() method will be automatically
        ///     called once upon framework initialization.
        /// </summary>
        /// <param name="canvas">
        ///     A reference to the automatically created
        ///     Canvas object that will be rendered by default.
        /// </param>
        public abstract void Init(Canvas canvas);

        /// <summary>
        ///     The Update() method will be called continuously
        ///     by the framework as part of the render loop.
        /// </summary>
        /// <param name="updateInfo">
        ///     A structure containing updated state information.
        /// </param>
        /// <returns>
        ///     True to continue running, false to end the loop.
        /// </returns>
        public abstract bool Update(UpdateInfo updateInfo);

        /// <summary>
        ///     The OnKeyEvent() method will be called
        ///     asynchrounously from the render loop any time
        ///     the user enters a key into the console.
        /// </summary>
        /// <param name="keyInfo">
        ///     A ConsoleKeyInfo object detailing the
        ///     console key event.
        /// </param>
        public abstract void OnKeyEvent(ConsoleKeyInfo keyInfo);
    }
}