using CameraDiplomat.Entities;

namespace CameraDiplomat.Services
{
	public class StatsService
	{
		private ConfigurationService _configurationService;
		private SessionCustomDesigner _sessionCustomDesigner;
		private SessionsDbService _sessionsDbService;

		public delegate void NewSessionFromDb();
		public delegate void MarriagePercentTooHigh();
		public delegate void MarriageCountInRowToBig();


		public event NewSessionFromDb EventNewSessionWasLoaded;
		public event MarriagePercentTooHigh EventBigMarriagePercent;
		public event MarriageCountInRowToBig EventBigMarriageCount;

		private string id;
		private int _totalCount;
		private int _marriageCount;
		private DateTime _startTime;
		private double _marriageProcent;

		private int _marriageCountInRow;


		private System.Timers.Timer timerToUploadActiveSessionPerTime;
		public StatsService(ConfigurationService configurationService, SessionCustomDesigner customDesigner, SessionsDbService sessionsDbService)
		{
			_configurationService = configurationService;
			_sessionCustomDesigner = customDesigner;
			_sessionsDbService = sessionsDbService;
			ResetStats();
			TimerInitialization();
		}
		public void TimerInitialization()
		{
			timerToUploadActiveSessionPerTime = new System.Timers.Timer();
			timerToUploadActiveSessionPerTime.Interval = _configurationService.tcpTimerInterval;
			timerToUploadActiveSessionPerTime.AutoReset = true;
			timerToUploadActiveSessionPerTime.Elapsed += async (e, arg) => await AutoUpdateOrCreateSession();
		}
		public void StartAutoUploadActiveSessionInDb()
		{
			timerToUploadActiveSessionPerTime.Start();
		}

		public string ResetStats()
		{
			id = Guid.NewGuid().ToString();
			_totalCount = 0;
			_marriageCount = 0;
			_startTime = DateTime.Now;
			return "Сессия обновлена";
		}

		public async Task AutoUpdateOrCreateSession()
		{
			if (_totalCount != 0)
				await _sessionsDbService.AddOrUpdateSession(GetEntitySession());
		}

		public Session GetEntitySession()
		{
			return _sessionCustomDesigner.DesigneNewEntity(id, _startTime, _totalCount, _marriageCount);
		}

		public void NewProductChecked(int successChecksCompleted)
		{
			_totalCount++;
			if (successChecksCompleted != _configurationService.totalChecksCount)
			{
				_marriageCount++;
				_marriageCountInRow++;
			}
			else
			{
				_marriageCountInRow = 0;
			}

			CalculatePercent();
		}

		public void LoadSessionFromDb(Session sessionFromDb)
		{
			id = sessionFromDb.Id;
			_totalCount = sessionFromDb.TotalCount;
			_marriageCount = sessionFromDb.MarriageCount;
			string[] startDate = sessionFromDb.Data.Split('/');
			_startTime = DateTime.Parse(startDate[0]);
			CalculatePercent();
			EnableMoniringMarriageCountInRow();
			EnableMoniringMarriagePercent();
		}

		private void CalculatePercent()
		{
			if (_marriageCount > 0)
			{
				_marriageProcent = (double)_marriageCount / _totalCount * 100;
			}
			else
			{
				_marriageProcent = 0;
			}
		}

		private void EnableMoniringMarriagePercent()
		{
			if (_configurationService.MonitorMarriagePercent)
			{
				if (_marriageProcent > _configurationService.MarriageMaxPercent & _totalCount > _configurationService.MinMathSet)
				{
					EventBigMarriagePercent?.Invoke();
				}
			}
		}

		private void EnableMoniringMarriageCountInRow()
		{
			if (_configurationService.MonitorMarriageCountInRow)
			{
				if (_marriageCountInRow > _configurationService.MarriageMaxCountInRow)
				{
					EventBigMarriageCount?.Invoke();
				}
			}
		}

		public string GetStatsString()
		{
			return $"Всего проверно {_totalCount} продуктов, из них {_marriageCount} брака (" +
			_marriageProcent.ToString("F2") + "%)";
		}

		public void InvokeUpdateSessionView()
		{
			EventNewSessionWasLoaded?.Invoke();
		}
	}
}
