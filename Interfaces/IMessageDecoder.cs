using CameraDiplomat.Services;
using CameraDiplomat.Entities;
using CameraDiplomat.DTO;

namespace CameraDiplomat.Interfaces
{
	public interface IMessageDecoder
	{
		event MessageDecoder.NonStandartMessage nonStandartMessage;
		Product DecodeMessege(string messageToDecode, out ProductViewModel viewModel, out int checksCompleted);
	}
}
