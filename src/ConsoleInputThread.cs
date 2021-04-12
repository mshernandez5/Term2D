using System;
using System.Collections.Generic;
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
        // Event Receivers
        private List<KeyInputListener> listeners;

        // For Managing Thread
        private Thread inputThread;
        private CancellationTokenSource tokenSource;

        public ConsoleInputThread()
        {
            listeners = new List<KeyInputListener>();
            inputThread = new Thread(inputLoop);
            tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Registers an event listener with the input thread,
        ///     allowing it to receive details whenever an event
        ///     occurs.
        /// </summary>
        /// <param name="listener">
        ///     An object implementing KeyInputListener to
        ///     receive console key events.
        /// </param>
        public void AddEventListener(KeyInputListener listener)
        {
            listeners.Add(listener);
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

        private void inputLoop()
        {
            ConsoleKeyInfo lastEvent;
            while (!tokenSource.Token.IsCancellationRequested)
            {
                // Wait For New Console Key Event
                lastEvent = Console.ReadKey(true);
                // Call Registered Event Handlers
                foreach (KeyInputListener listener in listeners)
                {
                    listener.OnKeyEvent(lastEvent);
                }
            }
        }
    }
}