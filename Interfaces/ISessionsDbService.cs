using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface ISessionsDbService
	{
		Task<bool> CreateSessionAsync(Session sessionToAddInDb);
		Task<bool> DeleteSessionAsync(Session sessionToAddInDb);
		List<Session> GetSessions();
		Task AddOrUpdateSession(Session sessionToAddOrUpdate);
		Task ClearSessionsDbAsync();
	}
}
