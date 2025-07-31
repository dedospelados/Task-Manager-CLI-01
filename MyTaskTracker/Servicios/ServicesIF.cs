using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BurbujaClass;

namespace TaskManager.ServicesInterfaces
{
    internal interface ServicesIF
    {
        Task<int> AddNewTask(String title);
        Task<bool> DeleteTask(int id);
        Task<bool> UpdateTask(int id, string title);
        Task<bool> SetStatus(string status, int id);
        //Task<string> ChangeDescription(string description); YA ME LO VOY A INVENTAR
        Task<List<BurbujaTask>> GetAllTasks();
        Task<List<BurbujaTask>> GetTaskByStatus(string status);
        List<string> GetHelp();
        

    }
}
