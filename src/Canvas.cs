using System;

namespace term2d
{
    /// <summary>
    ///     <para>
    ///         Representation of a 2D area with discretely
    ///         placed blocks to display in the console, each
    ///         one with a foreground and background color.
    ///     </para>
    ///     <para>
    ///         Includes methods to draw blocks over the Canvas
    ///         and render the representation in the console.
    ///     </para>
    /// </summary>
    public class Canvas
    {
        /// <summary>
        ///     The default character to draw for
        ///     empty blocks when the canvas is cleared
        ///     or initialized.
        /// </summary>
        public char DefaultBlock {set; get;}
        /// <summary>
        ///     The default foreground color to use for
        ///     new blocks with unspecified colors or
        ///     empty blocks when the canvas is cleared
        ///     or initialized.
        /// </summary>
        public ConsoleColor DefaultForegroundColor {set; get;}
        /// <summary>
        ///     The default background color to use for
        ///     new blocks with unspecified colors or
        ///     empty blocks when the canvas is cleared
        ///     or initialized.
        /// </summary>
        public ConsoleColor DefaultBackgroundColor {set; get;}

        // Internal Canvas Representation
        private char[,] blocks;
        private ConsoleColor[,,] colors;
        private int width;
        private int height;

        /// <summary>
        ///     Initialize a Canvas with default values.
        /// </summary>
        public Canvas()
        {
            DefaultBlock = ' ';
            DefaultForegroundColor = Console.ForegroundColor;
            DefaultBackgroundColor = Console.BackgroundColor;
            Initialize(Console.WindowWidth, Console.WindowHeight);
        }

        /// <summary>
        ///     Initialize a Canvas with provided values.
        /// </summary>
        /// <param name="width">
        ///     The width of the canvas.
        /// </param>
        /// <param name="height">
        ///     The height of the canvas.
        /// </param>
        /// <param name="block">
        ///     The default character to fill the canvas with.
        /// </param>
        /// <param name="fgColor">
        ///     The default color for new blocks.
        /// </param>
        /// <param name="bgColor">
        ///     The default background color for empty space.
        /// </param>
        public Canvas(int width, int height, char block, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            DefaultBlock = block;
            DefaultForegroundColor = fgColor;
            DefaultBackgroundColor = bgColor;
            Initialize(width, height);
        }

        /// <summary>
        ///     Initialize a Canvas with provided values.
        /// </summary>
        /// <param name="width">
        ///     The width of the canvas.
        /// </param>
        /// <param name="height">
        ///     The height of the canvas.
        /// </param>
        private void Initialize(int width, int height)
        {
            this.width = width;
            this.height = height;
            blocks = new char[height, width];
            colors = new ConsoleColor[height, width, 2];
            Clear(DefaultBlock, DefaultForegroundColor, DefaultBackgroundColor);
        }

        /// <summary>
        ///     Clears the canvas with the given settings.
        /// </summary>
        /// <param name="block">
        ///     The default character to fill the canvas with.
        /// </param>
        /// <param name="fgColor">
        ///     The default color for new blocks.
        /// </param>
        /// <param name="bgColor">
        ///     The default background color for empty space.
        /// </param>
        public void Clear(char block, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    blocks[row, col] = block;
                    colors[row, col, 0] = bgColor;
                    colors[row, col, 1] = fgColor;
                }
            }
        }

        /// <summary>
        ///     Clears the canvas using default settings.
        /// </summary>
        public void Clear()
        {
            Clear(DefaultBlock, DefaultForegroundColor, DefaultBackgroundColor);
        }

        /// <summary>
        ///     Get the width of the canvas, independent
        ///     of console window size.
        /// </summary>
        /// <returns>
        ///     The width of the canavs.
        /// </returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        ///     Get the height of the canvas, independent
        ///     of console window size.
        /// </summary>
        /// <returns>
        ///     The height of the canavs.
        /// </returns>
        public int GetHeight()
        {
            return height;
        }

        /// <summary>
        ///     <para>
        ///         Renders the visible portion of the
        ///         canvas in the console.
        ///     </para>
        ///     <para>
        ///         Not recommended for manual use, let the
        ///         framework game loop render Canvas objects.
        ///     </para>
        /// </summary>
        public void Render()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor lastBg = colors[0, 0, 0];
            ConsoleColor lastFg = colors[0, 0, 1];
            char[] displayBuffer = new char[(width + 1) * height];
            int bufferPtr = 0;
            for (int row = 0; row < height && row < Console.WindowHeight; row++)
            {
                for (int col = 0; col < width && col < Console.WindowWidth; col++)
                {
                    char block = blocks[row, col];
                    ConsoleColor bgColor = colors[row, col, 0];
                    ConsoleColor fgColor = colors[row, col, 1];
                    if (bgColor != lastBg || fgColor != lastFg)
                    {
                        Console.BackgroundColor = lastBg;
                        Console.ForegroundColor = lastFg;
                        Console.Write(displayBuffer, 0, bufferPtr);
                        bufferPtr = 0;
                        lastBg = bgColor;
                        lastFg = fgColor;
                    }
                    displayBuffer[bufferPtr++] = block;
                }
                if (row != height - 1 && (!OperatingSystem.IsWindows() || height < Console.WindowHeight))
                {
                    displayBuffer[bufferPtr++] = '\n';
                }
            }
            Console.BackgroundColor = lastBg;
            Console.ForegroundColor = lastFg;
            Console.Write(displayBuffer, 0, bufferPtr);
        }

        /// <summary>
        ///     Draw a single block at the specified
        ///     row and column.
        /// </summary>
        public void Draw(int row, int col, char block, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            blocks[row, col] = block;
            colors[row, col, 0] = bgColor;
            colors[row, col, 1] = fgColor;
        }

        /// <summary>
        ///     Draw a single block at the specified
        ///     row and column with default colors.
        /// </summary>
        public void Draw(int row, int col, char block)
        {
            blocks[row, col] = block;
            colors[row, col, 0] = DefaultBackgroundColor;
            colors[row, col, 1] = DefaultForegroundColor;
        }
    }
}