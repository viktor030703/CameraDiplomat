using CameraDiplomat.DTO;
using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class MessageDecoder : IMessageDecoder
	{
		private readonly ConfigurationService _configurationService;
		public MessageDecoder(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}

		//message must to have next format: <strat>Cavivar<quality>false<percent>50<codeIs>123456789abcdef<textIs>none<end>
		public Product DecodeMessege(string messageToDecode, out ProductViewModel viewModel, out int checksCompleted)
		{
			string[] messages;

			bool isQualityOk = false;
			string qualityDescription = "брак";
			bool isTextOk = false;
			bool isCodeOK = false;
			checksCompleted = 0;
			
			try
			{
				messages = messageToDecode.Split(_configurationService.SymbolsByWichWeSplit);
				if (messages.Count() == _configurationService.PartsCodeIncludeAfterSplit)
				{
					Product newProduct = new Product();
					newProduct.Id = Guid.NewGuid().ToString();
					newProduct.productName = messages[2];
					newProduct.quality = Boolean.Parse(messages[4]);
					newProduct.percent = Int32.Parse(messages[6]);
					newProduct.code = messages[8];
					newProduct.text = messages[10];
					newProduct.data = DateTime.Now.ToString();
					newProduct.loginUserWichLeaveProduct = _configurationService.activeUser.login;
					if(newProduct.quality)
					{
						isQualityOk = true;
						qualityDescription = "хорошее";
						checksCompleted++;
					}

					if(newProduct.text.Contains(_configurationService.TextPattern))
					{
						isTextOk = true;
						checksCompleted++;
					}

					if(newProduct.code.Length == _configurationService.CodeLength)
					{
						isCodeOK = true;
						checksCompleted++;
					}

					viewModel = new ProductViewModel(true, isQualityOk, isCodeOK, isTextOk,
						newProduct.productName, qualityDescription,newProduct.code, newProduct.text);

					return newProduct;
				}
				else
				{
					Log.Warning("Нестандартное собщение от камеры:" + messageToDecode);
					viewModel = null;
					return null;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Исключение во время декодирования" + ex.Message.ToString());
				viewModel = null;
				return null;
			}
		}
	}
}
