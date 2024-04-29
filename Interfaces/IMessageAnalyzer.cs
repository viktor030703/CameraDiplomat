using CameraDiplomat.Services;

namespace CameraDiplomat.Interfaces
{
	public interface IMessageAnalyzer
	{
		event MessageAnalyzer.NonStandartMessage nonStandartMessage;
		string[] DecodeMessege(string messageToDecode);
	}
}
