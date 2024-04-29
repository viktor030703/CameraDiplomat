using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class NonStandartMessageHandler : INonStandartMessageHandler
	{
		private SynchronizationContext _UISynchronizationContext;

		private readonly ConfigurationService _configurationService;
		private readonly IMessageAnalyzer _messageAnalyzer;
		private readonly ICameraConnectionService _cameraConnectionService;

		public delegate void NeedUserReaction(string message);
		public event NeedUserReaction needUserReaction;

		private bool IsConnectionSuccess;
		public NonStandartMessageHandler(ConfigurationService configurationService, ICameraConnectionService cameraConnectionService, IMessageAnalyzer messageAnalyzer)
		{
			_configurationService = configurationService;
			_messageAnalyzer = messageAnalyzer;
			_cameraConnectionService = cameraConnectionService;

			_messageAnalyzer.nonStandartMessage += NewNonStandartMessage;

			_UISynchronizationContext = SynchronizationContext.Current;
		}

		public async Task NewNonStandartMessage(string nonStandartMessage)
		{
			Log.Information("Получено нестандартное сообщение");

			if (nonStandartMessage == _configurationService.CameraLoginRequirement)
			{
				Log.Information("Собщение о требовании авторизации га сервере");
				_configurationService.CameraNeedAutohorise = true;
				if (_configurationService.CameraAutoLogin)
				{
					IsConnectionSuccess = await _cameraConnectionService.TryAutoConnectCamera(nonStandartMessage);

					if(!IsConnectionSuccess)
					{
						_configurationService.CameraNeedAutohorise = true;
						Log.Error("Не получается подключится, вывожу сообщение пользователю");
						needUserReaction?.Invoke(nonStandartMessage);
					}
				}
				else
				{
					Log.Warning("Неизвестное сообщение, вывожу его пользователю");
					needUserReaction?.Invoke(nonStandartMessage);
				}
			}
			else//Add Switch!!!
			{
				Log.Warning("Неизвестное сообщение, вывожу его пользователю");
				_UISynchronizationContext.Post(new SendOrPostCallback(o =>
				{
					needUserReaction?.Invoke(nonStandartMessage);
				}), null);
			}
		}
	}
}
