using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Entities
{
	public class Product
	{
		public string Id { get; set; }
		public string productName { get; set; }
		public string loginUserWichLeaveProduct { get; set; }
		public bool quality { get; set; }
		public int percent { get; set; }
		public string code { get; set; }
		public string text { get; set; }
		public string data { get; set; }
	}
}
