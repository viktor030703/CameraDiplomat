namespace CameraDiplomat.Interfaces
{
	public interface ICameraConnectionService
	{
		Task<bool> TryAutoConnectCamera(string messageFromCamera);
	}
}
