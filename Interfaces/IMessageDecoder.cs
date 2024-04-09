using CameraDiplomat.Entities;
using CameraDiplomat.DTO;

namespace CameraDiplomat.Interfaces
{
	public interface IMessageDecoder
	{
		Product DecodeMessege(string messageToDecode, out ProductViewModel viewModel, out int checksCompleted);
	}
}
