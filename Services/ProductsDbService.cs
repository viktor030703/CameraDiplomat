using CameraDiplomat.Context;
using CameraDiplomat.Entities;

namespace CameraDiplomat.Services
{
	public class ProductsDbService
	{
		ApplicationContext _db;

		public ProductsDbService(ApplicationContext db)
		{
			_db = db;
		}
		public void CheckDbExist()
		{
			_db.Database.EnsureCreated();
			_db.SaveChanges();
		}

		public async Task CreateProductAsync(Product newProduct)
		{
			_db.Products.Add(newProduct);
			await _db.SaveChangesAsync();
		}

		public async Task ChangeProductAsync(Product oldProduct, Product newProduct)
		{
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
		}

		public async Task DeleteProductAsync(Product userToDelete)
		{
			_db.Products.Remove(userToDelete);
			await _db.SaveChangesAsync();
		}

		public List<Product> GetProducts()
		{
			return _db.Products.ToList();
		}

		public async Task ClearProductDbAsync()
		{
			var table = _db.Set<Product>();
			_db.RemoveRange(table);
			await _db.SaveChangesAsync();
		}
	}
}
