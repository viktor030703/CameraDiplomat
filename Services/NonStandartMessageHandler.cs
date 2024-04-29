using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class NonStandartMessageHandler : INonStandartMessageHandler
	{
		private readonly ConfigurationService _configurationService;
		private readonly IMessageDecoder _messageDecoder;
		private readonly ICameraConnectionService _cameraConnectionService;

		public delegate void NeedUserReaction(string message);
		public event NeedUserReaction needUserReaction;

		private bool IsConnectionSuccess;
		public NonStandartMessageHandler(ConfigurationService configurationService, ICameraConnectionService cameraConnectionService, IMessageDecoder messageDecoder)
		{
			_configurationService = configurationService;
			_messageDecoder = messageDecoder;
			_cameraConnectionService = cameraConnectionService;

			_messageDecoder.nonStandartMessage += NewNonStandartMessage;
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
						needUserReaction.Invoke(nonStandartMessage);
					}
				}
				else
				{
					Log.Warning("Неизвестное сообщение, вывожу его пользователю");
					needUserReaction.Invoke(nonStandartMessage);
				}
			}
		}
	}
}
