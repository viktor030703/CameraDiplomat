using CameraDiplomat.Services;

namespace CameraDiplomat.Interfaces
{
	public interface INonStandartMessageHandler
	{
		event NonStandartMessageHandler.NeedUserReaction needUserReaction;

		Task NewNonStandartMessage(string nonStandartMessage);
	}
}
