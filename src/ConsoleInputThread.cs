using System;
using System.Threading;

namespace term2d
{
    /// <summary>
    ///     Silently reads keys entered into the console to take user
    ///     input, runs in a separate thread to avoid blocking the
    ///     main thread.
    /// </summary>
    class ConsoleInputThread
    {
        // For Keeping Track Of Input, Read Only From Outside Classes
        private ConsoleKey lastInput;
        private bool hasUnreadInput;

        // For Managing Thread
        private Thread inputThread;
        private CancellationTokenSource tokenSource;

        public ConsoleInputThread()
        {
            lastInput = ConsoleKey.F;
            hasUnreadInput = false;
            inputThread = new Thread(inputLoop);
            tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Start taking user input.
        /// </summary>
        public void Start()
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
        public void Stop()
        {
            tokenSource.Cancel();
        }

        /// <summary>
        ///     Returns the last ConsoleKey read from the user,
        ///     no guarantees the input is new.
        /// </summary>
        public ConsoleKey ReadLast()
        {
            hasUnreadInput = false;
            return lastInput;
        }

        /// <summary>
        ///     Returns true if an unread input is available,
        ///     otherwise false.
        /// </summary>
        public bool HasUnreadInput()
        {
            return hasUnreadInput;
        }

        private void inputLoop()
        {
            while (!tokenSource.Token.IsCancellationRequested)
            {
                lastInput = Console.ReadKey(true).Key;
                hasUnreadInput = true;
            }
        }
    }
}