using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
    public class LoggerService : ILoggerService, IDisposable
	{
		private readonly ConfigurationService _configurationService;
		private System.Timers.Timer timerToLogs = new System.Timers.Timer();

		public LoggerService(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
			UpdateSaveLogs();
			TimerInitializationAndStart();
		}

		private void TimerInitializationAndStart()
		{
			timerToLogs.Interval = _configurationService.LogsTimerInterval;
			timerToLogs.AutoReset = true;
			timerToLogs.Elapsed += (e, args) => UpdateSaveLogs();
			timerToLogs.Start();
		}

		public void UpdateSaveLogs()
		{
			Log.CloseAndFlush();
			Log.Logger = new LoggerConfiguration()
			.WriteTo.File(_configurationService.PathToLoggerFile, fileSizeLimitBytes: 100000000, rollOnFileSizeLimit: true)
			.CreateLogger();
		}

		public void Dispose()
		{
			timerToLogs.Stop();
			timerToLogs.Dispose();
			Log.CloseAndFlush();
		}
	}

}
