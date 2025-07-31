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
        Task<int> AddNewTask(String Title);
        Task<bool> DeleteTask(int id,string Title);
        Task<bool> UpdateTask(int id, string Title);
        Task<bool> SetStatus(int Id, string Description);
        Task<List<BurbujaTask>> GetAllTasks();
        Task<List<BurbujaTask>> GetTaskByStatus(string Status);
        List<string> GetHelp();

    }
}
