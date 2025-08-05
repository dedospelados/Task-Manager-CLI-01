using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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
        }private static Task<List<BurbujaTask>> GetTasksFromjson() 
        {
            string TasksFromJsonString = File.ReadAllText(FilePath);
            if (!string.IsNullOrEmpty(TasksFromJsonString)) 
            {
                return Task.FromResult(JsonSerializer.Deserialize<List<BurbujaTask>>(TasksFromJsonString) ?? []);
            }
            return Task.FromResult(new List<BurbujaTask>());
        }
        //i think the getalltasks method is redundant as it has the same workings that 
        //the one above it and it should be changed in smeway to make it
        //worth having it or just straight up remove it
        public Task<List<BurbujaTask>> GetAllTasks() 
        {
            try
            {

                if (!File.Exists(FilePath)) 
                {   
                    System.Threading.Tasks.Task.FromResult(new List<BurbujaTask>());
                }
                string jsonString = File.ReadAllText(FilePath);

                if (!string.IsNullOrEmpty(jsonString))
                {
                List<BurbujaTask> burbujasLista = JsonSerializer.Deserialize<List<BurbujaTask>>(jsonString);
                return System.Threading.Tasks.Task.FromResult(burbujasLista ?? []);
                //i think there will be an exception as the deserialized may return
                //a null value and it should be accounted for, having that line above
                //the if statement
                }
                else {
                    return System.Threading.Tasks.Task.FromResult(new List<BurbujaTask>());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }public Task<bool> SetStatus(int id,string status)
        {
            if (!File.Exists(FilePath)) 
            { 
                return Task.FromResult(false);
            }
            
            var tasksFromJson = GetTasksFromjson();

            if (tasksFromJson.Result.Count >0)
            {
                var taskToBeUpd = tasksFromJson.Result.Where(x => x.Id == id).SingleOrDefault();
                if(taskToBeUpd !=null)
                {
                    var updTask = new BurbujaTask
                    {
                        Id = id,
                        Title = taskToBeUpd.Title,
                        Description = taskToBeUpd.Description,
                        DateUpd = DateTime.Now,
                        TaskStatus = GetStatusToSet()

                    };

                    tasksFromJson.Result.Remove(taskToBeUpd);
                    tasksFromJson.Result.Add(updTask);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }
               
            }
            
            return Task.FromResult(false);
        }private Status GetStatusToSet(string status)
        {
            switch (status)
            {
                case "mark-in-progress":
                    return Status.ToDone;
                case "mark-done":
                    return Status.Done;
                case "mark-todo":
                    return Status.ToDone;
                default:
                    return Status.ToDone;
            }
        }



    }
 }

