using CameraDiplomat.Services;
using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface IStatisticService
	{
		public event StatisticService.NewSessionFromDb EventNewSessionWasLoaded;
		public event StatisticService.MarriagePercentTooHigh EventBigMarriagePercent;
		public event StatisticService.MarriageCountInRowToBig EventBigMarriageCountInRow;


		void TimerInitialization();
		void TimerIntervalUpdate();
		void StartAutoUploadActiveSessionInDb();
		string ResetStats();
		Task AutoUpdateOrCreateSession();
		Session GetEntitySession();
		void NewProductChecked(int successChecksCompleted);
		void LoadSessionFromDb(Session sessionFromDb);
		string GetStatsString();
		void InvokeUpdateSessionView();
	}
}
