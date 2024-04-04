using CameraDiplomat.Context;
using CameraDiplomat.Entities;
using Microsoft.EntityFrameworkCore;


namespace CameraDiplomat.Services
{
	public class ProductsDbService
	{
		ApplicationContext db = new ApplicationContext();

	
		public void CheckDbExist()
		{
			db.Database.EnsureCreated();
			db.SaveChanges();
		}

		public async Task CreateProductAsync(Product newProduct)
		{
			db.Products.Add(newProduct);
			await db.SaveChangesAsync();
		}

		public async Task ChangeProductAsync(Product oldProduct, Product newProduct)
		{
			await db.Products.FindAsync(oldProduct);
			db.Products.Update(oldProduct);

			if(!String.IsNullOrEmpty(newProduct.Id)) 
				oldProduct.Id = newProduct.Id;

			if (!String.IsNullOrEmpty(newProduct.productName))
				oldProduct.productName = newProduct.productName;

			if (!String.IsNullOrEmpty(newProduct.text))
				oldProduct.text = newProduct.text;

			
			oldProduct.percent = newProduct.percent;
			oldProduct.quality = newProduct.quality;

			await db.SaveChangesAsync();

		}

		public async Task DeleteProductAsync(Product userToDelete)
		{
			db.Products.Remove(userToDelete);
			await db.SaveChangesAsync();
		}

		public List<Product> GetProducts()
		{
			return db.Products.ToList();
		}


		public async Task ClearProductDbAsync()
		{

			var table = db.Set<Product>();

			db.RemoveRange(table);
			await db.SaveChangesAsync();
		}
	}
	
		




}
