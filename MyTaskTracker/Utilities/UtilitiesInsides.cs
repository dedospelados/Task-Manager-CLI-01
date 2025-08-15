using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskManager.UtilitiesInsides
{
    public static class UtilitiesInsides
    {
        public static void PrintInfoMessage(string message)
        {
            //Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n"+message);
            Console.ResetColor();
        }public static void PrintHelpMessage(string message)
        {
           Console.ForegroundColor = ConsoleColor.DarkBlue;
           Console.WriteLine("\n"+message);
           Console.ResetColor();
        }public static void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;   
            Console.WriteLine("\n"+message);    
            Console.ResetColor();
        }public static void PrintNumberMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }public static void PrintCommandMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }
        public static List<string> ParseInput(string input)
        {
            var commandArgs = new List<string>();

            var regex = new Regex(@"[\""].+?[\""]|[^ ]+");
            var matches = regex.Matches(input);

            foreach(Match match in matches)
            {
                //remove surrounding quotes if any
                string value = match.Value.Trim('"');
                commandArgs.Add(value);
            }

            return commandArgs;
        }
        public static void ClearConsole()
        {
            Console.Clear();
        }

    }

}
