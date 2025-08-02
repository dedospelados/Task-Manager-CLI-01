using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public int GetTaskId() 
        {
            if (!File.Exists(FilePath))
            {
                return 1;
            }
            else 
            {
                string tasksFromJsonFileString = File.ReadAllText(FilePath);
                if (!string.IsNullOrEmpty(tasksFromJsonFileString)) 
                {
                    var burbujero = JsonSerializer.Deserialize<List<BurbujaTask>>(tasksFromJsonFileString);
                    if (burbujero != null && burbujero.Count > 0) 
                    {
                        return burbujero.OrderBy(x => x.Id).Last().Id + 1;
                    }
                }
            }
            return 331443;
        }
        //i also want to make it so that u can delete a task by title
        public Task<bool> DeleteTask(int id)
        {
            if (!File.Exists(FilePath)) 
            {
                return Task.FromResult(false);
            }
            var tasksFromJson = GetTasksFromJson();

            if (tasksFromJson.Result.Count > 0) 
            {
                var taskToBeDeleted = tasksFromJson.Result
                    .Where(x => x.Id == id).SingleOrDefault();
                //this two lines above are a single line sliced to make it
                //more readable
                if (taskToBeDeleted != null) 
                {
                    tasksFromJson.Result.Remove(taskToBeDeleted);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }
            }
            
            return Task.FromResult(false);
        }   //region helper method note to blow it to the bottom after
            //completing all methods and stuff
        private static void UpdateJsonFile(Task<List<BurbujaTask>> tasksFromJson) 
        {
            string updatedBurbujas = JsonSerializer.Serialize<List<BurbujaTask>>(tasksFromJson.Result);
            File.WriteAllText(FilePath, updatedBurbujas);
        }

    }
 }

