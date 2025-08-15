using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskManager.BurbujaClass;
using TaskManager.ServicesInterfaces;
using TaskManager.StatusEnum;

namespace TaskManager.Services
{
    public class ServicesInsides : ServicesIF //SetStatus
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
                Console.WriteLine("hubo un problema inflando esta burbuja, el error " + ex.Message);
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
        private static Task<List<BurbujaTask>> GetTasksFromJson()
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
                else
                {
                    return System.Threading.Tasks.Task.FromResult(new List<BurbujaTask>());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }public Task<List<BurbujaTask>> GetTaskByStatus(string status)
        {
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(new List<BurbujaTask>());

            }string JsonString = File.ReadAllText(FilePath);

            if (!string.IsNullOrEmpty(JsonString))
            {
                var burbujas = JsonSerializer.Deserialize<List<BurbujaTask>>(JsonString);
                var statusToCheck = GetStatusToDisplay(status);
                return Task.FromResult(burbujas?.Where(x => x.TaskStatus == statusToCheck).ToList() ?? []);
            }

            return Task.FromResult(new List<BurbujaTask>());
        }
        public Task<bool> SetStatus(string status, int id )
        {
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(false);
            }

            var tasksFromJson = GetTasksFromJson();

            if (tasksFromJson.Result.Count > 0)
            {
                var taskToBeUpd = tasksFromJson.Result.Where(x => x.Id == id).SingleOrDefault();
                if (taskToBeUpd != null)
                {
                    var updTask = new BurbujaTask
                    {
                        Id = id,
                        Title = taskToBeUpd.Title,
                        Description = taskToBeUpd.Description,
                        DateUpd = DateTime.Now,
                        TaskStatus = GetStatusToDisplay(status)
                        //i changed the last line from the original tasktracker
                        //so that it uses only 1 method instead of having two that
                        //do the same thing, i havent tested it so ill keep both methods
                    };

                    tasksFromJson.Result.Remove(taskToBeUpd);
                    tasksFromJson.Result.Add(updTask);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }

            }

            return Task.FromResult(false);
        }
        private Status GetStatusToSet(string status)
        {
            switch (status)
            {
                case "mark-todo":
                    return Status.ToDone;
                case "mark-in-progress":
                    return Status.DoningIt;
                case "mark-done":
                    return Status.Done;
                default:
                    return Status.ToDone;
            }
        }
        private Status GetStatusToDisplay(string status)
        {
            switch (status)
            {
                case "todo":
                    return Status.ToDone;
                case "in-progress":
                    return Status.DoningIt;
                case "done":
                    return Status.Done;
                default:
                    return Status.ToDone;
            }


        }public Task<bool> UpdateTitle(int id,string title)
        {
            if (!File.Exists(FilePath))
            {
                return Task.FromResult(false);
            }var tasksFromJson = GetTasksFromJson();

            if (tasksFromJson.Result.Count > 0)
            {
                var taskToBeUpd = tasksFromJson.Result.Where(x => x.Id == id).SingleOrDefault();
                if(taskToBeUpd != null)
                {
                    var updTask = new BurbujaTask
                    {
                        Id = taskToBeUpd.Id,
                        Title = title,
                        Description = taskToBeUpd.Description,
                        DateUpd = taskToBeUpd.DateUpd,
                        TaskStatus = taskToBeUpd.TaskStatus
                    };
                    tasksFromJson.Result.Add(updTask);
                    tasksFromJson.Result.Remove(taskToBeUpd);
                    UpdateJsonFile(tasksFromJson);
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);

        }public new List<string> GetHelp()
        {
            return new List<string>
            {
                "add \"Task Description\" - To add a new task, type add with task description",
                "update \"Task Id\" \"Task Description\" - To update a task, type update with task id and task description",
                "delete \"Task Id\" - To delete a task, type delete with task id",
                "mark-in-progress \"Task Id\" - To mark a task to in progress, type mark-in-progress with task id",
                "mark-done \"Task Id\" - To mark a task to done, type mark-done with task id",
                "list - To list all task with its current status",
                "list done - To list all task with done status",
                "list todo  - To list all task with todo status",
                "list in-progress  - To list all task with in-progress status",
                "exit - To exit from app",
                "clear - To clear console window"
            };
        }
        #region helper methods
        private bool CreateFileIfNotExist()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    using (FileStream fs = File.Create(FilePath))
                    {
                        Console.WriteLine($"Archivo {FileName} creatinado succesfulimente");
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Archivo {FileName} erroneamente no posible creatinarlo, error" + ex.Message);
                return false;
            }
            #endregion
        }
    
    }
}
 

