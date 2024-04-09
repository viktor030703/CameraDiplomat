using CameraDiplomat.Context;
using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class ProductsDbService : IProductsDbService
	{
		private readonly ApplicationContext _db;
		private readonly ConfigurationService _configurationService;

		public ProductsDbService(ApplicationContext db, ConfigurationService configurationService)
		{
			_db = db;
			_configurationService = configurationService;
		}

		public async Task CreateProductAsync(Product newProduct)
		{
			await _db.dbAccessSemaphore.WaitAsync();
			_db.Products.Add(newProduct);
			await _db.SaveChangesAsync();
			_db.dbAccessSemaphore.Release();
		}

		public async Task ChangeProductAsync(Product oldProduct, Product newProduct)
		{
			await _db.dbAccessSemaphore.WaitAsync();
			await _db.Products.FindAsync(oldProduct);
			_db.Products.Update(oldProduct);

			if(!String.IsNullOrEmpty(newProduct.Id)) 
				oldProduct.Id = newProduct.Id;

			if (!String.IsNullOrEmpty(newProduct.productName))
				oldProduct.productName = newProduct.productName;

			if (!String.IsNullOrEmpty(newProduct.text))
				oldProduct.text = newProduct.text;
	
			oldProduct.percent = newProduct.percent;
			oldProduct.quality = newProduct.quality;

			await _db.SaveChangesAsync();
			_db.dbAccessSemaphore.Release();
			Log.Warning(_configurationService.activeUser.login + " измененяет продукт");
		}

		public async Task DeleteProductAsync(Product userToDelete)
		{
			await _db.dbAccessSemaphore.WaitAsync();
			_db.Products.Remove(userToDelete);
			await _db.SaveChangesAsync();
			_db.dbAccessSemaphore.Release();
			Log.Warning(_configurationService.activeUser.login + " удаляет продукт");
		}

		public List<Product> GetProducts()
		{
			_db.dbAccessSemaphore.Wait();
			_db.dbAccessSemaphore.Release();
			return _db.Products.ToList();
		}

		public async Task ClearProductDbAsync()
		{
			await _db.dbAccessSemaphore.WaitAsync();
			var table = _db.Set<Product>();
			_db.RemoveRange(table);
			await _db.SaveChangesAsync();
			_db.dbAccessSemaphore.Release();
			Log.Warning(_configurationService.activeUser.login + " удаляет всю базу продуктов");
		}
	}
}
