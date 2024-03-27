using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Services
{
	
	public class ConfigurationService
	{
		public string ActiveUserRole = "admin";

		public string CameraIP = "127.0.0.3";
		public int CameraPort = 8080;

		public string PathToDb;

		public string errorColor = "red";
		public string trueColor = "green";


		public ConfigurationService() 
		{
			PathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CamerasDimplomat.db");
		}
	}
}
