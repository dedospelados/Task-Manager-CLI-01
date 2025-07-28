using System;
using TaskManager.StatusEnum;

namespace TaskTracker.BurbujaClass
{

	public class Burbuja
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime DateCreated { get; set; }
		public string Description { get; set; } = string.Empty;
		public Status TaskStatus { get; set; }
		
        //public DateTime ReviveEvery { get; set; }
        //public DateTime DateUpd { get; set; }
        //public DateTime Expiration  { get; set; }
    }
}