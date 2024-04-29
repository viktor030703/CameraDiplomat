using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class MessageAnalyzer : IMessageAnalyzer
	{

		public delegate Task NonStandartMessage(string message);
		public event NonStandartMessage nonStandartMessage;

		private readonly ConfigurationService _configurationService;
		public MessageAnalyzer(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}

		//default message must to have next format:
		//_strat_Cavivar_quality_false_percent_50_codeIs_123456789abcdef_textIs_none_end_
		public string[] DecodeMessege(string messageToDecode)
		{
			string[] messages;
			try
			{
				messages = messageToDecode.Split(_configurationService.SymbolsByWichWeSplit);
				if (messages.Count() == _configurationService.PartsCodeIncludeAfterSplit)
				{
					return messages;
				}
				else
				{
					Log.Warning("Нестандартное собщение от камеры:" + messageToDecode);
					////nen rjyntrcn cby[hjybpfwbb
					nonStandartMessage?.Invoke(messageToDecode);
					return null;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Исключение во время декодирования" + ex.Message.ToString());
				nonStandartMessage?.Invoke(messageToDecode);
				return null;
			}
		}
	}
}