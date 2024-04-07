using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Entities
{
	public class Session
	{
		public string Id { get; set; }
		public int TotalCount { get; set; }
		public int MarriageCount { get; set; }
		public string MarriagePercent { get; set; }
		public string Data { get; set; }
		public string creatorsLogin { get; set; }
	}
}
