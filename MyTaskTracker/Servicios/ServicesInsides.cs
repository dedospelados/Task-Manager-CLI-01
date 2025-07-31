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
                var burbujero = new List<BurbujaTask>();
                var burbuja = new BurbujaTask
                {
                    Id = GetTaskId(),
                    Title = title,
                    DateCreated = DateTime.Now,
                    DateUpd = DateTime.Now,
                    TaskStatus = Status.ToDone
                };

                var fileCreationSuccessful = CreateFileIfNotExist();

                if (fileCreationSuccessful)

                {
                    string tasksFromJsonFileString = File.ReadAllText(FilePath);
                    if (!string.IsNullOrEmpty(tasksFromJsonFileString))
                    {
                        burbujero = JsonSerializer.Deserialize<List<BurbujaTask>>(tasksFromJsonFileString);
                    }

                    burbujero?.Add(burbuja);
                    string updatedBurbujero = JsonSerializer.Serialize<List<BurbujaTask>>(burbujero ?? new List<BurbujaTask>());
                    File.WriteAllText(FilePath, updatedBurbujero);
                    return Task.FromResult(burbuja.Id);
                }
                return Task.FromResult(0);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("hubo un problema inflando esta burbuja, el error "+ ex.Message);
                return Task.FromResult(0);

            }
            
        }
    }
 }

