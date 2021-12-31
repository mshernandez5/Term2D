using System;
using System.Diagnostics;

namespace Term2D
{
    /// <summary>
    ///     The core of the framework, initializes
    ///     the application and controls the render loop.
    /// </summary>
    public class Term2D
    {
        /// <summary>
        ///     The active canvas to render on screen.
        /// </summary>
        public static Canvas ActiveCanvas = new Canvas();
        /// <summary>
        ///     The maximum FPS the render loop will attempt to reach.
        /// </summary>
        public static int TargetFPS = 60;
        /// <summary>
        ///     When true, disables frame limits and instead
        ///     attempts to reach the maximum possible framerate.
        /// </summary>
        public static bool UnlimitedFPS = false;

        /// <summary>
        ///     Starts the provided application using the
        ///     framework.
        /// </summary>
        public static void Start(Term2DApplication app)
        {
            // Configure The Console Based On Operating System
            InitConfig configurer;
            if (OperatingSystem.IsWindows())
            {
                configurer = new WindowsInitConfig();
            }
            else
            {
                configurer = new DefaultInitConfig();
            }
            configurer.initialize();
            // Initialize Application
            app.Init(ActiveCanvas);
            // Begin Render Loop
            RenderLoop(app);
        }

        /// <summary>
        ///     The primary render loop.
        /// </summary>
        private static void RenderLoop(Term2DApplication app)
        {
            // Prepare Additional Threads
            ConsoleInputThread inputThread = new ConsoleInputThread();

            // Start Additional Threads
            inputThread.Start();
            inputThread.AddEventListener(app);

            // Remember Window Size To Detect Changes
            int lastWindowWidth = Console.WindowWidth;
            int lastWindowHeight = Console.WindowHeight;

            // Render Loop Variables
            long startTimestamp;
            long targetTicks;
            long measuredTicks = 0L;

            // Update Information Struct
            UpdateInfo updateInfo = new UpdateInfo();

            // Begin Loop
            bool runLoop = true;
            while (runLoop)
            {
                startTimestamp = Stopwatch.GetTimestamp();
                if (UnlimitedFPS)
                {
                    targetTicks = 0;
                }
                else
                {
                    targetTicks = Stopwatch.Frequency / TargetFPS;
                }
                // Update State Information
                updateInfo.ActiveCanvas = ActiveCanvas;
                updateInfo.DeltaTime = measuredTicks / (double) Stopwatch.Frequency;
                // Run Application Update Loop
                runLoop = app.Update(updateInfo);
                // Render
                if (Console.WindowWidth != lastWindowWidth || Console.WindowHeight != lastWindowHeight)
                {
                    Console.Clear();
                    lastWindowWidth = Console.WindowWidth;
                    lastWindowHeight = Console.WindowHeight;
                }
                ActiveCanvas.Render();
                // If Finished Early, Wait For Consistent Frame Time
                do
                {
                    measuredTicks = System.Diagnostics.Stopwatch.GetTimestamp() - startTimestamp;
                } while (targetTicks > measuredTicks);
            }

            // Stop Threads
            inputThread.Stop();
        }
    }
}