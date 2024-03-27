using CameraDiplomat.Context;
using CameraDiplomat.Entities;


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

		public void CreateProduct(ProductDataDecoded newProduct)
		{
			db.Products.Add(newProduct);
			db.SaveChanges();
		}

		public void ChangeProductDataDecoded(ProductDataDecoded oldProduct, ProductDataDecoded newProduct)
		{
			db.Products.Find(oldProduct);

			if(!String.IsNullOrEmpty(newProduct.Id)) 
				oldProduct.Id = newProduct.Id;

			if (!String.IsNullOrEmpty(newProduct.productName))
				oldProduct.productName = newProduct.productName;

			if (!String.IsNullOrEmpty(newProduct.text))
				oldProduct.text = newProduct.text;

			//Непредсказуемое поведение
			//if (newProduct.percent != null)
			//	oldProduct.percent = newProduct.percent;

			//if (newProduct.quality != null)
			//	oldProduct.quality = newProduct.quality;



		}

		public void DeleteProduct(ProductDataDecoded userToDelete)
		{
			db.Products.Remove(userToDelete);
			db.SaveChanges();
		}

		public List<ProductDataDecoded> GetProducts()
		{
			return db.Products.ToList();
		}

	}
	
		


}
