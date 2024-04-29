using CameraDiplomat.DTO;
using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface IProductQualityAnalyzer
	{
		public ProductViewModel CheckProductQuality(Product product, out int successfulChecksCount);
	}
}
