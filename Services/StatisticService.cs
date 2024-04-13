using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class StatisticService : IStatisticService
	{
		private readonly ConfigurationService _configurationService;
		private readonly ISessionCustomDesigner _sessionCustomDesigner;
		private readonly ISessionsDbService _sessionsDbService;

		public delegate void NewSessionFromDb();
		public delegate void MarriagePercentTooHigh();
		public delegate void MarriageCountInRowToBig();

		public event NewSessionFromDb EventNewSessionWasLoaded;
		public event MarriagePercentTooHigh EventBigMarriagePercent;
		public event MarriageCountInRowToBig EventBigMarriageCountInRow;

		private string id;
		private int _totalCount;
		private int _marriageCount;
		private DateTime _startTime;
		private double _marriageProcent;

		private int _marriageCountInRow;


		private System.Timers.Timer timerToUploadActiveSessionPerTime;
		public StatisticService(ConfigurationService configurationService, ISessionCustomDesigner customDesigner, ISessionsDbService sessionsDbService)
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
			timerToUploadActiveSessionPerTime.Interval = _configurationService.DbTimerInterval;
			timerToUploadActiveSessionPerTime.AutoReset = true;
			timerToUploadActiveSessionPerTime.Elapsed += async (e, arg) => await AutoUpdateOrCreateSession();
		}

		public void TimerIntervalUpdate()
		{
			timerToUploadActiveSessionPerTime.Interval = _configurationService.DbTimerInterval;
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
			Log.Warning(_configurationService.activeUser.login + "обновляет сессию");
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
			if (successChecksCompleted != _configurationService.TotalChecksCount)
			{
				_marriageCount++;
				_marriageCountInRow++;
			}
			else
			{
				_marriageCountInRow = 0;
			}
			CalculatePercent();
			EnableMoniringMarriagePercent();
			EnableMoniringMarriageCountInRow();
		}

		public void LoadSessionFromDb(Session sessionFromDb)
		{
			id = sessionFromDb.Id;
			_totalCount = sessionFromDb.TotalCount;
			_marriageCount = sessionFromDb.MarriageCount;
			string[] startDate = sessionFromDb.Data.Split('/');
			_startTime = DateTime.Parse(startDate[0]);
			CalculatePercent();
			Log.Warning(_configurationService.activeUser.login + "загружает сессию из БД");
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
					EventBigMarriageCountInRow?.Invoke();
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
