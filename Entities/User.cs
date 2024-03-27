using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Entities
{
	public class User
	{
		public string id {  get; set; }
		public string login {  get; set; }
		public string password { get; set; }
		public string role { get; set; }
		public string lastLoginData {  get; set; }

	}
}
