using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class CameraConnectionService : ICameraConnectionService
	{
		private readonly ConfigurationService _configurationService;
		private readonly ITCPService _tcpService;
		private string cameraResponce;

		public CameraConnectionService(ConfigurationService configurationService, ITCPService tcpService)
		{
			_configurationService = configurationService;
			_tcpService = tcpService;
		}

		public async Task<bool> TryAutoConnectCamera(string messageFromCamera)
		{
			try
			{
				_tcpService.StopSemaphores();
				for (int i = 0; i < _configurationService.CameraConnectionAttemps; i++)
				{
					Log.Information("Попытка автологина на сервере");

					if (messageFromCamera == _configurationService.CameraLoginRequirement)
					{
						Thread.Sleep(_configurationService.TimeBeforeOperations);
						cameraResponce = await _tcpService.SendMessageAndGetResponce(_configurationService.CameraLogin);
						if (cameraResponce == _configurationService.CameraPasswordRequirement)
						{
							Thread.Sleep(_configurationService.TimeBeforeOperations);
							cameraResponce = await _tcpService.SendMessageAndGetResponce(_configurationService.CameraPassword);
							if (cameraResponce == _configurationService.CameraConnectedMessage)
							{
								_configurationService.CameraNeedAutohorise = false;
								_tcpService.ResumeSemaphores();
								Log.Information("Успешно подключено");
								return true;
							}
							else
							{
								Log.Information("Не удалось подключиться - ошибка сверки пароля");
								Thread.Sleep(_configurationService.TimeBeforeNextConnectionAttempt);
							}
						}
						else
						{
							Log.Information("Не удалось подключиться - ошибка сверки логина");
						}
					}
					else
					{
						Log.Information("Не удалось подключиться это сообщние не требование логина и пароля");
						_configurationService.CameraNeedAutohorise = false;
					}
				}
				_tcpService.ResumeSemaphores();
				Log.Information("Попытки подключения исчеркпаны");
				return false;
			}
			catch (Exception ex)
			{
				Log.Error("Критическая ошибка во время автологина" + ex.Message.ToString());
				_tcpService.ResumeSemaphores();
				return false;
			}
		}
	}
}