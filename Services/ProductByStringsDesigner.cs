using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class ProductByStringsDesigner : IProductByStringsDesigner
	{
		private readonly ConfigurationService _configurationService;
		public ProductByStringsDesigner(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}
		public Product CreateProductByMessages(string[] messages)
		{
			try
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
				return newProduct;
			}
			catch (Exception ex)
			{
				Log.Error("Exception during the creation product by string[]: " + ex.Message.ToString());
				return null;
			}

		}
	}
}
