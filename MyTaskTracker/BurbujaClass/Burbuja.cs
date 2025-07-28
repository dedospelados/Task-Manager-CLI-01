using System;
using TaskStatus.Enum;

namespace BurbujaClass
{

	public class Burbuja
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime DateCreated { get; set; }
		public string Descrption { get; set; }
		public Status TaskStatus { get; set; }
		
        //public DateTime ReviveEvery { get; set; }
        //public DateTime DateUpd { get; set; }
        //public DateTime Expiration  { get; set; }
    }
}