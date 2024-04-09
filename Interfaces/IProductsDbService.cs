using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface IProductsDbService
	{
		Task CreateProductAsync(Product newProduct);
		Task ChangeProductAsync(Product oldProduct, Product newProduct);
		Task DeleteProductAsync(Product userToDelete);
		List<Product> GetProducts();
		Task ClearProductDbAsync();
	}
}
