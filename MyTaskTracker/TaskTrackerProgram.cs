using Microsoft.VisualBasic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Task_Manager_CLI_01
{

    public class Racistas
    {
        public string? Name { get; set; }

        public int RaceNumber { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class TaskTrackerProgram
    {
       
        static void Main(string[] args)
        {

            Racistas ElGoat = new Racistas
            {
                Name = "Max",
                RaceNumber = 33,
                DateTime = DateTime.UtcNow
            };

            Console.WriteLine("clave?");

            string JsonifiedObj = JsonSerializer.Serialize(ElGoat);

        }
    }
}
