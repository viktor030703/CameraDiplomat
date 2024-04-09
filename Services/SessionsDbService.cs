using CameraDiplomat.Context;
using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CameraDiplomat.Services
{
	public class SessionsDbService : ISessionsDbService
	{
		private readonly ApplicationContext _db;
		private readonly ConfigurationService _configurationService;
		public SessionsDbService(ApplicationContext db, ConfigurationService configurationService)
		{
			_db = db;
			_configurationService = configurationService;
		}

		public async Task<bool> CreateSessionAsync(Session sessionToAddInDb)
		{

			if (sessionToAddInDb != null)
			{
				await _db.dbAccessSemaphore.WaitAsync();
				_db.Sessions.Add(sessionToAddInDb);
				await _db.SaveChangesAsync();
				_db.dbAccessSemaphore.Release();
				Log.Information(_configurationService.activeUser.login + " создает новую сессию");
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
				await _db.dbAccessSemaphore.WaitAsync();
				_db.Sessions.Update(sessionToAddInDb);
				_db.Remove(sessionToAddInDb);
				await _db.SaveChangesAsync();
				_db.dbAccessSemaphore.Release();
				Log.Information(_configurationService.activeUser.login + " удаляет сессию");
				return true;
			}
			else
			{
				return false;
			}
		}

		public List<Session> GetSessions()
		{
			_db.dbAccessSemaphore.Wait();
			List<Session> sessions = _db.Sessions.ToList();
			_db.dbAccessSemaphore.Release();
			return sessions;
		}

		public async Task AddOrUpdateSession(Session sessionToAddOrUpdate)
		{
			try
			{
				await _db.dbAccessSemaphore.WaitAsync();
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
					_db.dbAccessSemaphore.Release();
				}
				else
				{
					await _db.Sessions.AddAsync(sessionToAddOrUpdate);
					await _db.SaveChangesAsync();
					_db.dbAccessSemaphore.Release();
				}
			}
			catch (Exception ex)
			{
				_db.dbAccessSemaphore.Release();
				Log.Information("исключение во время добавления сессии");
			}
		}

		public async Task ClearSessionsDbAsync()
		{
			await _db.dbAccessSemaphore.WaitAsync();
			var table = _db.Set<Session>();
			_db.RemoveRange(table);
			await _db.SaveChangesAsync();
			_db.dbAccessSemaphore.Release();
			Log.Warning(_configurationService.activeUser.login + " очищает всю базу данных сессий");
		}
	}
}
