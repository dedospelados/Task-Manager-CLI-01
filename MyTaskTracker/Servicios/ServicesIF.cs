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
        Task<bool> UpdateTitle(int id, string title);
        //Task<bool> UpdateDescription(string description); YA ME LO VOY A INVENTAR
        Task<bool> SetStatus(string status, int id);
        Task<List<BurbujaTask>> GetAllTasks();
        List<string> GetHelp();
        

    }
}
