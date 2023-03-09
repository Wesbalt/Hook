using System;
using System.Text;

namespace Hook
{
    public static class Logger
    {
        private const LoggingLevel level = LoggingLevel.Debug;

        public static void Err(object value, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(LoggingLevel.Err, value, values);
        }

        public static void Warn(object value, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log(LoggingLevel.Warn, value, values);
        }

        public static void Info(object value, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log(LoggingLevel.Info, value, values);
        }

        public static void Debug(object value, params object[] values)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Log(LoggingLevel.Debug, value, values);
        }

        private static void Log(LoggingLevel lvl, object value, params object[] values)
        {
            if (level < lvl) {
                Console.ResetColor();
                return;
            }
            try
            {
                if (values.Length == 0)  Console.WriteLine(value.ToString());
                else                     Console.WriteLine(String.Format(value.ToString(), values));
            }
            finally
            {
                Console.ResetColor();
            }
        }

        enum LoggingLevel
        {
            Quiet, Err, Warn, Info, Debug
        }
    }
}
