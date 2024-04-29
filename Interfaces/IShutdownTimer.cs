using CameraDiplomat.Services;

namespace CameraDiplomat.Interfaces
{
	public interface IShutdownTimer
	{
		event ShutdownTimer.TimeToShutdownLine timeToShutdownLineEvent;

		void StopAndCloseTimer();

		void SendTime();
	}

}