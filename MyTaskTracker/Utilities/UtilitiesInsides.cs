using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager_CLI_01.Utilities
{
    public class UtilitiesInsides
    {
        static string Comun(string stuacion)
        {
            string situacion = "laTipica";
            switch (situacion)
            {
                case "laTipica":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
            } 
            return "pilin";
        }
        public string AmbienceChange() 
        {
                              
            
            return "pilin";
        }
        
    }
}
