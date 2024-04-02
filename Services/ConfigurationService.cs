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

		//public string CameraIP = "10.162.3.240";
		public int CameraPort = 6000;
		public string CameraIP = "192.168.1.3";


		public string PathToDb;

		public string errorColor = "red";
		public string trueColor = "green";


		public ConfigurationService() 
		{
			PathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CamerasDimplomat.db");
		}

		public void GetCameraIPandPort(out string ip, out int port)
		{
			ip = CameraIP;
			port = CameraPort;
		}
	}
}
