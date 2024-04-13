using Serilog;
using CameraDiplomat.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CameraDiplomat.Services
{
	public class ConfigurationService
	{
		public User activeUser;

		//TCP settings
		public int CameraPort = 6000;
		public string CameraIP = "10.162.0.99";
		public int TcpTimeout = 250;
		public string LastMessageToServer;

		//Cognex Camera Settings
		public bool CameraNeedAutohorise = false;
		public bool CameraAutoLogin = true;
		public string CameraLoginRequirement = "login";
		public string CameraPasswordRequirement = "password";
		public string CameraConnectedMessage = "ok";
		public string CameraRetryMessage = "try again";
		public string CameraLogin = "admin";
		public string CameraPassword = "is2801c";
		public int CameraConnectionAttemps = 3;
		public int TimeBeforeOperations = 50;
		public int TimeBeforeNextConnectionAttempt = 50;

		//File IO settings
		private string pathFolderWithData;
		public string PathToJson;
		public string PathToLoggerFile;
		public string PathToDb;

		public int LogsTimerInterval = 10000;
		public int DbTimerInterval = 9000;

		//Application configuration
		public bool IsSettingFromJSON = false;
		public bool IsDbFatalError = false;
		public bool SaveDataInAppFolder = true;
		public bool IsCameraConnected = false;

		//Для контроля качества
		public bool MonitorMarriagePercent = false;
		public bool MonitorMarriageCountInRow = false;
		public int PartsCodeIncludeAfterSplit = 13;
		public string TextPattern = "Hello";
		public char[] SymbolsByWichWeSplit = new char[] { '<', '>' };
		public int CodeLength = 10;
		public int TotalChecksCount = 3;

		public int MarriageMaxCountInRow = 3;
		public float MarriageMaxPercent = 60;
		public int MinMathSet = 10;

		public bool SoundsOn = true;
		public ConfigurationService()
		{
			PathCreator();
			AddNewWaringLog("Запуск приложения - инициализация сервисов");
			CreateEmptyUser();
			IsSettingFromJSON = TryLoadConfigurationFromJson();
			if(!IsSettingFromJSON)
				CreateNewJsonFile();
		}

		private bool TryLoadConfigurationFromJson()
		{
			if (File.Exists(PathToJson))
			{
				try
				{
					string configJsonInString = File.ReadAllText(PathToJson);
					JObject json = JObject.Parse(configJsonInString);

					IsDbFatalError = Boolean.Parse(json["IsDbFatalError"].ToString());
					SaveDataInAppFolder = Boolean.Parse(json["SaveDataInAppFolder"].ToString());

					CameraAutoLogin = Boolean.Parse(json["CameraAutoLogin"].ToString());
					CameraPassword = json["CameraPassword"].ToString();
					CameraLogin = json["CameraLogin"].ToString();
					CameraLoginRequirement = json["CameraLoginRequirement"].ToString();
					CameraPasswordRequirement = json["CameraPasswordRequirement"].ToString();
					CameraRetryMessage = json["CameraRetryMessage"].ToString();
					CameraConnectionAttemps = Int32.Parse(json["CameraConnectionAttemps"].ToString());
					CameraConnectedMessage = json["CameraConnectedMessage"].ToString();
					TimeBeforeOperations = Int32.Parse(json["TimeBeforeOperations"].ToString());
					TimeBeforeNextConnectionAttempt = Int32.Parse(json["TimeBeforeNextConnectionAttempt"].ToString());

					CameraIP = json["CameraIP"].ToString();
					CameraPort = Int32.Parse(json["CameraPort"].ToString());
					TcpTimeout = Int32.Parse(json["TcpTimeout"].ToString());
					LastMessageToServer = json["LastMessageToServer"].ToString();

					LogsTimerInterval = Int32.Parse(json["LogsTimerInterval" ].ToString());
					DbTimerInterval = Int32.Parse(json["DbTimerInterval"].ToString());

					MonitorMarriageCountInRow = Boolean.Parse(json["MonitorMarriageCountInRow"].ToString());
					TextPattern = json["TextPattern"].ToString();
					MonitorMarriagePercent = Boolean.Parse(json["MonitorMarriagePercent"].ToString());
					CodeLength = Int32.Parse(json["CodeLength"].ToString());
					TotalChecksCount = Int32.Parse(json["TotalChecksCount"].ToString());
					MinMathSet = Int32.Parse(json["MinMathSet"].ToString());

					SoundsOn = Boolean.Parse(json["SoundsOn"].ToString());

					return true;
				}
				catch (Exception ex)
				{
					AddNewFatalLog("Исключение! В вашем json файле содержится ошибка: " + ex.Message.ToString() +
						" исправьте её или удалите конфигурацию!");
					Environment.Exit(1);
					return false;
				}
			}
			else
			{
				AddNewWaringLog("JSON файл не существует!");
				return false;
			}
		}

		public void CreateNewJsonFile()
		{
			try
			{
			string jsonConfigString = JsonConvert.SerializeObject(this);
			File.WriteAllText(PathToJson, jsonConfigString);
			}
			catch(Exception ex)
			{
				AddNewFatalLog("Исключение - невозможно создать JSON файл: " + ex.Message.ToString());
			}
		}

		private void PathCreator()
		{
			if (SaveDataInAppFolder)
			{
				pathFolderWithData = AppDomain.CurrentDomain.BaseDirectory;

				//Now use hardcode :-C
				//now we have: ...\CameraDiplomat\bin\Release\net7.0-windows10.0.19041.0\win10-x64\AppX\
				//but want: ...\CameraDiplomat
				//Let's do

				string previousPath;
				for (int i = 0; i < 6; i++)
				{
					previousPath = Directory.GetParent(pathFolderWithData).FullName;
					pathFolderWithData = previousPath;
				}

				PathToDb = Path.Combine(pathFolderWithData, "CamerasDimplomat.db");
				PathToJson = Path.Combine(pathFolderWithData, "AppConfiguretion.json");
				PathToLoggerFile = Path.Combine(pathFolderWithData, "Logs.txt");
			}
			else
			{
				//Also, we can save all data in user documents folder AND IT IS APPROPRIATE!!!
				pathFolderWithData = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

				PathToDb = Path.Combine(pathFolderWithData, "CamerasDimplomat.db");
				PathToJson = Path.Combine(pathFolderWithData, "AppConfiguretion.json");
				PathToLoggerFile = Path.Combine(pathFolderWithData, "Logs.txt");
			}
		}

		public async Task<bool> SaveCofigurationInJSONAsync()
		{
			try
			{
				string jsonFileSerializedContent = JsonConvert.SerializeObject(this);
				await File.WriteAllTextAsync(PathToJson, jsonFileSerializedContent);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public void GetCameraIPandPort(out string ip, out int port)
		{
			ip = CameraIP;
			port = CameraPort;
		}

		public void CreateEmptyUser()
		{
			activeUser = new User()
			{
				login = "anonim",
				role = "admin",
			};
		}

		private void AddNewWaringLog(string message)
		{
			using (LoggerService loggerService = new LoggerService(this))
			{
				Log.Warning(message);
			}
		}

		private void AddNewFatalLog(string message)
		{
			using (LoggerService loggerService = new LoggerService(this))
			{
				Log.Fatal(message);
			}
		}

	}
}
