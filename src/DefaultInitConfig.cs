using System;

namespace term2d
{
    /// <summary>
    ///     Performs default console initialization
    ///     procedures for UNIX-like systems.
    /// </summary>
    class DefaultInitConfig : InitConfig
    {
        public void initialize()
        {
            Console.CursorVisible = false;
            Console.Clear();
        }
    }
}