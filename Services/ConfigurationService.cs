using CameraDiplomat.Context;
using CameraDiplomat.Entities;
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
		public User activeUser; 

		//public string CameraIP = "10.162.3.240";
		public int CameraPort = 6000;
		public string CameraIP = "192.168.246.198";//"10.162.0.114";


		public string PathToDb;

		public string errorColor = "red";
		public string trueColor = "green";

		//Для контроля качества
		public string textPattern = "Hello";
		public int codeLength = 10;

		public int totalChecksCount = 3;

		public int DbTimerInterval = 9000;
		public int tcpTimerInterval = 9000;

		public bool IsCameraConnected;

		public bool MonitorMarriageCountInRow = false;
		public bool MonitorMarriagePercent = false;

		public int MarriageMaxCountInRow = 5;
		public float MarriageMaxPercent = 50;
		public int MinMathSet = 10;
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
