using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Entities
{
	public class ProductDataDecoded
	{
			public string Id { get; set; }
			public string productName { get; set; }
			public bool quality { get; set; }
			public int percent { get; set; }
			public string code { get; set; }
			public string text { get; set; }
	}
}
