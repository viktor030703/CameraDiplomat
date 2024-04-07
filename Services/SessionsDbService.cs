using CameraDiplomat.Context;
using CameraDiplomat.Entities;
using Microsoft.EntityFrameworkCore;

namespace CameraDiplomat.Services
{
	public class SessionsDbService
	{
		private ApplicationContext _db;

		public SessionsDbService(ApplicationContext db)
		{
			_db = db;
		}

		public async Task<bool> CreateSessionAsync(Session sessionToAddInDb)
		{
			if (sessionToAddInDb != null)
			{
				_db.Sessions.Add(sessionToAddInDb);
				await _db.SaveChangesAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> DeleteSessionAsync(Session sessionToAddInDb)
		{

			if (sessionToAddInDb != null)
			{
				_db.Sessions.Update(sessionToAddInDb);
				_db.Remove(sessionToAddInDb);
				await _db.SaveChangesAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<List<Session>> GetSessionsAsync()
		{
			return await _db.Sessions.ToListAsync();
		}

		public async Task AddOrUpdateSession(Session sessionToAddOrUpdate)
		{
			try
			{

				Session sessionFromDb = await _db.Sessions.FirstOrDefaultAsync(i => i.Id == sessionToAddOrUpdate.Id);
				if (sessionFromDb != null)
				{
					_db.Sessions.Update(sessionFromDb);
					sessionFromDb.MarriagePercent = sessionToAddOrUpdate.MarriagePercent;
					sessionFromDb.creatorsLogin = sessionToAddOrUpdate.creatorsLogin;
					sessionFromDb.Data = sessionToAddOrUpdate.Data;
					sessionFromDb.TotalCount = sessionToAddOrUpdate.TotalCount;
					sessionFromDb.MarriageCount = sessionToAddOrUpdate.MarriageCount;

					await _db.SaveChangesAsync();
					//может быть косяк
				}
				else
				{
					await _db.Sessions.AddAsync(sessionToAddOrUpdate);
					await _db.SaveChangesAsync();
					//и тут
				}
			}
			catch (Exception ex)
			{

			}
		}
	}
}
