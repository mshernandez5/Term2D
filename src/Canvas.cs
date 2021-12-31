using System;

namespace Term2D
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

        /// <summary>
        ///     The width of the canvas, independent
        ///     of console window size.
        /// </summary>
        public int Width {get; private set;}

        /// <summary>
        ///     The height of the canvas, independent
        ///     of console window size.
        /// </summary>
        public int Height {get; private set;}

        // Internal Canvas Representation
        private char[,] blocks;
        private ConsoleColor[,,] colors;

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
            this.Width = width;
            this.Height = height;
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
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
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
            int renderHeight = Math.Min(Height, Console.WindowHeight);
            int renderWidth = Math.Min(Width, Console.WindowWidth);
            char[] displayBuffer = new char[(Width + 1) * Height];
            int bufferPtr = 0;
            for (int row = 0; row < renderHeight; row++)
            {
                for (int col = 0; col < renderWidth; col++)
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
                if (row != renderHeight - 1)
                {
                    displayBuffer[bufferPtr++] = '\n';
                }
            }
            Console.BackgroundColor = lastBg;
            Console.ForegroundColor = lastFg;
            Console.Write(displayBuffer, 0, bufferPtr);
            Console.ResetColor();
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

        /// <summary>
        ///     Draws a string horizontally starting
        ///     from the specified row and column.
        /// </summary>
        public void DrawText(int row, int startCol, string text, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            for (int offset = 0; offset < text.Length; offset++)
            {
                int col = startCol + offset;
                blocks[row, col] = text[offset];
                colors[row, col, 0] = bgColor;
                colors[row, col, 1] = fgColor;
            }
        }

        /// <summary>
        ///     Draws a string horizontally starting
        ///     from the specified row and column
        ///     using default colors.
        /// </summary>
        public void DrawText(int row, int startCol, string text)
        {
            for (int offset = 0; offset < text.Length; offset++)
            {
                int col = startCol + offset;
                blocks[row, col] = text[offset];
                colors[row, col, 0] = DefaultBackgroundColor;
                colors[row, col, 1] = DefaultForegroundColor;
            }
        }
    }
}