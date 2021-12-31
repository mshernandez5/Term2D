﻿using System;
using System.Diagnostics;

namespace Term2D
{
    /// <summary>
    ///     The core of the framework, initializes
    ///     the game and controls the game loop.
    /// </summary>
    public class Term2D
    {
        /// <summary>
        ///     The active canvas to render on screen.
        /// </summary>
        public static Canvas ActiveCanvas = new Canvas();
        /// <summary>
        ///     The maximum FPS the game will attempt to reach.
        /// </summary>
        public static int TargetFPS = 60;
        /// <summary>
        ///     When true, disables frame limits and instead
        ///     attempts to reach the maximum possible framerate.
        /// </summary>
        public static bool UnlimitedFPS = false;

        /// <summary>
        ///     Starts the provided game using the
        ///     framework.
        /// </summary>
        public static void Start(Game game)
        {
            Console.WriteLine("[INFO]: Starting term2D Framework...");
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
            // Run Game Specific Initialization
            game.Init(ActiveCanvas);
            // Begin Game Loop
            GameLoop(game);
            // Stop
            Console.WriteLine("\n[INFO]: Exiting term2D Framework...");
        }

        /// <summary>
        ///     The primary game loop.
        /// </summary>
        private static void GameLoop(Game game)
        {
            // Prepare Additional Threads
            ConsoleInputThread inputThread = new ConsoleInputThread();

            // Start Additional Threads
            inputThread.Start();
            inputThread.AddEventListener(game);

            // Remember Window Size To Detect Changes
            int lastWindowWidth = Console.WindowWidth;
            int lastWindowHeight = Console.WindowHeight;

            // Game Loop Variables
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
                // Update Information For Game To Read
                updateInfo.ActiveCanvas = ActiveCanvas;
                updateInfo.DeltaTime = measuredTicks / (double) Stopwatch.Frequency;
                // Update Game
                runLoop = game.Update(updateInfo);
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