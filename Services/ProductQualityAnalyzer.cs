using CameraDiplomat.DTO;
using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class ProductQualityAnalyzer : IProductQualityAnalyzer
	{
		private readonly ConfigurationService _configurationService;
		public ProductQualityAnalyzer(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}
		public ProductViewModel CheckProductQuality(Product product, out int successfulChecksCount)
		{
			bool isQualityOk = false;
			string qualityDescription = "брак";
			bool isTextOk = false;
			bool isCodeOK = false;

			successfulChecksCount = 0;

			try
			{
				if (product.text.Contains(_configurationService.TextPattern))
				{
					isTextOk = true;
					successfulChecksCount++;
				}

				if (product.code.Length == _configurationService.CodeLength)
				{
					isCodeOK = true;
					successfulChecksCount++;
				}

				if (product.quality)
				{
					isQualityOk = true;
					qualityDescription = "хорошее";
					successfulChecksCount += 1;
				}

				ProductViewModel productViewModel = new ProductViewModel(true, isQualityOk, isCodeOK, isTextOk,
				product.productName, qualityDescription, product.code, product.text);

				return productViewModel;
			}
			catch (Exception ex)
			{
				Log.Error("Exception during creation product view model" + ex.Message.ToString());
				return null;
			}
		}

	}

}
