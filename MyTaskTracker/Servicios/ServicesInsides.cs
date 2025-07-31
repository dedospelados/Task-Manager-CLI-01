using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManager.BurbujaClass;
using TaskManager.ServicesInterfaces;
using TaskManager.StatusEnum;

namespace TaskManager.Services
{
    public class ServicesInsides : ServicesIF
    {
        private static string FileName = "BurbujaData.json";
        private static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

        public Task<int> AddNewTask(string title)
        {
            try 
            {
                var burbujasTasks = new List<BurbujaTask>();
                var burbuja = new BurbujaTask
                {
                    Id = GetTaskId(),
                    Title = title,
                    DateCreated = DateTime.Now,
                    DateUpd = DateTime.Now,
                    TaskStatus = Status.ToDone
                };
                
                var FileCreationSuccessful = CreateFileIfNotExist();
                
                if (FileCreationSuccessful)
               
                {
                    string tasksFromJsonFileString = File.ReadAllText(FilePath);
                    if (!string.IsNullOrEmpty(tasksFromJsonFileString)) 
                    {
                        burbujasTasks = JsonSerializer.Deserialize<List<BurbujaTask>>(tasksFromJsonFileString);
                    }

                    burbujasTasks?.Add(burbuja);
                    string updatedBurbujas = JsonSerializer.Serialize<List<BurbujaTask>>(burbujasTasks ?? new List<BurbujaTask>());
                    File.WriteAllText(FilePath, updatedBurbujas);
                    return Task.FromResult(burbuja.Id);
                }
                return Task.FromResult(0);
            }
            return null;    
        }
    }
}
