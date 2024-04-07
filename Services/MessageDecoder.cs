using CameraDiplomat.DTO;
using CameraDiplomat.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Services
{
	public class MessageDecoder
	{
		private ConfigurationService _configurationService;

		public MessageDecoder(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}

		//message must to have next format: <strat>Cavivar<quality>false3<percent>50<codeIs>123456789abcdef<textIs>none<end>
		public Product MessegeDiplomat(string messageToDiplomating, out ProductViewModel viewModel, out int checksCompleted)
		{
			string[] messages;

			bool isQualityOk = false;
			string qualityDescription = "брак";
			bool isTextOk = false;
			bool isCodeOK = false;
			checksCompleted = 0;
			
			try
			{
				messages = messageToDiplomating.Split('<', '>');
				if (messages.Count() == 13)
				{
					Product newProduct = new Product();
					newProduct.Id = Guid.NewGuid().ToString();
					newProduct.productName = messages[2];
					newProduct.quality = Boolean.Parse(messages[4]);
					newProduct.percent = Int32.Parse(messages[6]);
					newProduct.code = messages[8];
					newProduct.text = messages[10];
					newProduct.data = DateTime.Now.ToString();

					if(newProduct.quality)
					{
						isQualityOk = true;
						qualityDescription = "хорошее";
						checksCompleted++;
					}

					if(newProduct.text.Contains(_configurationService.textPattern))
					{
						isTextOk = true;
						checksCompleted++;
					}

					if(newProduct.code.Length == _configurationService.codeLength)
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
					viewModel = null;
					return null;
				}
			}
			catch (Exception ex)
			{
				viewModel = null;
				return null;
			}
		}
	}
}
