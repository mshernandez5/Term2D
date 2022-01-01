using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Term2D
{
    /// <summary>
    ///     Silently reads keys entered into the console to take user
    ///     input, runs in a separate thread to avoid blocking the
    ///     main thread.
    /// </summary>
    class ConsoleInputThread
    {
        /// <summary>
        ///     An event dispatched whenever a new console
        ///     key input is read from the console.
        /// </summary>
        public event EventHandler<ConsoleKeyEventArgs> RaiseConsoleKeyEvent;

        // For Managing Thread
        private Thread inputThread;
        private CancellationTokenSource tokenSource;

        internal ConsoleInputThread()
        {
            inputThread = new Thread(inputLoop);
            tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Start taking user input.
        /// </summary>
        internal void Start()
        {
            if (!inputThread.IsAlive && tokenSource.IsCancellationRequested)
            {
                tokenSource.Dispose();
                tokenSource = new CancellationTokenSource();
            }
            inputThread.Start();
        }

        /// <summary>
        ///     Stop taking user input.
        /// </summary>
        internal void Stop()
        {
            tokenSource.Cancel();
        }

        private void inputLoop()
        {
            while (!tokenSource.Token.IsCancellationRequested)
            {
                ConsoleKeyInfo e = Console.ReadKey(true);
                EventHandler<ConsoleKeyEventArgs> raiseEvent = RaiseConsoleKeyEvent;
                if (raiseEvent != null)
                {
                    raiseEvent(this, new ConsoleKeyEventArgs(e));
                }
            }
        }
    }
}