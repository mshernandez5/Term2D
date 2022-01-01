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
        // Event Receivers
        private ConcurrentDictionary<KeyInputListener, bool> listeners;

        // For Managing Thread
        private Thread inputThread;
        private CancellationTokenSource tokenSource;

        public ConsoleInputThread()
        {
            listeners = new ConcurrentDictionary<KeyInputListener, bool>();
            inputThread = new Thread(inputLoop);
            tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Registers an event listener with the input thread,
        ///     allowing it to receive details whenever an event
        ///     occurs.
        ///     <br/>
        ///     This method is thread-safe.
        /// </summary>
        /// <param name="listener">
        ///     An object implementing KeyInputListener to
        ///     receive console key events.
        /// </param>
        public void AddEventListener(KeyInputListener listener)
        {
            listeners.TryAdd(listener, true);
        }

        /// <summary>
        ///     Stop sending key events to an event listener.
        ///     <br/>
        ///     This method is thread-safe.
        /// </summary>
        /// <param name="listener">
        ///     The listener to stop sending events to.
        /// </param>
        /// <returns>
        ///     True if the listener was found and unregistered from events.
        /// </returns>
        public bool RemoveEventListener(KeyInputListener listener)
        {
            return listeners.TryRemove(listener, out _);
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
                foreach (var entry in listeners)
                {
                    entry.Key.OnKeyEvent(lastEvent);
                }
            }
        }
    }
}