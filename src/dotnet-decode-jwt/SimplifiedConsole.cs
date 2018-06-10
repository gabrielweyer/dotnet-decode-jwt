using System;

namespace DotNet.Decode.Jwt
{
    public interface IConsole
    {
        ConsoleColor ForegroundColor { set; }
        void WriteLine(string value);
        void ResetColor();
    }

    class SimplifiedConsole : IConsole
    {
        public ConsoleColor ForegroundColor
        {
            set => Console.ForegroundColor = value;
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}
