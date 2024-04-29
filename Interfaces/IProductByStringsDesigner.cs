using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface IProductByStringsDesigner
	{
		public Product CreateProductByMessages(string[] messages);

	}
}
