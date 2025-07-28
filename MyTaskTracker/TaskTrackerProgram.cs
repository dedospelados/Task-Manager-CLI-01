using Microsoft.VisualBasic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using BurbujaClass;

namespace Task_Manager_CLI_01
{

    
    
    public class TaskTrackerProgram
    {
        static void WelcomeUser()
        {
            Console.WriteLine("let me manage ur task broski");
        }

        static void Main(string[] args)
        {

            //Console.WriteLine("Want me to manage some task for u? \nEnter help to see a list of commands");

            WelcomeUser();


            Console.ReadKey(true);
        }
    }
}

