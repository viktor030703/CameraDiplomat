using CameraDiplomat.Services;
namespace CameraDiplomat.Interfaces
{
	public interface ITCPService
	{
		event TCPService.NewMessageGetter EventNewMessageGet;
		event TCPService.MesssageCameraUnavalible EventCameraUnavalible;
		event TCPService.CameraConnectedSuccessfully EventCameraConnectedSuccessfully;
		void ConnectToCamera();
		void CameraConnectHandler();
		void CameraDisconnectHandler();
		bool BooleanCheckCameraStatus();
		void GetMessages();
		Task<bool> SendMessage(string message);
		Task<string> SendMessageAndGetResponce(string message);
		void StopSemaphores();
		void ResumeSemaphores();
	}
}
