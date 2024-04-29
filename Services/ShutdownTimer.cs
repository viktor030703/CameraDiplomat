using CameraDiplomat.Interfaces;

namespace CameraDiplomat.Services
{
	public class ShutdownTimer : IShutdownTimer
	{
		SynchronizationContext uiCintext;

		private readonly ConfigurationService _configurationService;
		private readonly ITCPService _tcpService;

		private System.Timers.Timer _timerToUpdateValue;

		public delegate void TimeToShutdownLine(int value);
		public event TimeToShutdownLine timeToShutdownLineEvent;

		private int secondBeforeShutDown;
		private int timeToUpdateTime = 1000;

		public ShutdownTimer(ConfigurationService configurationService, ITCPService tcpService)
		{
			_configurationService = configurationService;
			_tcpService = tcpService;
		
			_timerToUpdateValue = new System.Timers.Timer();

			uiCintext = SynchronizationContext.Current;
			secondBeforeShutDown = _configurationService.TimeToShutdownLine / timeToUpdateTime;
			TimerToUpdateValueInitializationAndStart();
		}


		public void TimerToUpdateValueInitializationAndStart()
		{
			_timerToUpdateValue.AutoReset = true;
			_timerToUpdateValue.Interval = timeToUpdateTime;
			_timerToUpdateValue.Elapsed += (sender, e) => SendTime();
			_timerToUpdateValue.Start();

		}


		public void StopAndCloseTimer()
		{
			_timerToUpdateValue.Stop();
			_timerToUpdateValue.Close();
		}

		public void SendTime()
		{
			secondBeforeShutDown--;
			uiCintext.Post(new SendOrPostCallback(o=> {
				timeToShutdownLineEvent?.Invoke(secondBeforeShutDown);
			}),null);
		}

	}
}
