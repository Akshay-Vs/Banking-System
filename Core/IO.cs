using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.IO
{
    public static class IO
    {
        public static ConsoleColor ForegroundColor = ConsoleColor.Green;
        public static ConsoleColor ErrorColor = ConsoleColor.Red;

        public static void WriteError(string format, object arg0) => Console.WriteLine(format, arg0, Console.ForegroundColor = ErrorColor);
        public static void WriteLine(string format) => Console.WriteLine(format, Console.ForegroundColor = ForegroundColor);
        public static void WriteLine(string format, object arg0) => Console.WriteLine(format, arg0, Console.ForegroundColor = ForegroundColor);
        public static void Write(string format) => Console.Write(format, Console.ForegroundColor = ForegroundColor);
    }
}
