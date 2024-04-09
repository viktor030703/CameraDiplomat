using CameraDiplomat.Context;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class DbMainController : IDbMainController
	{
		private readonly ApplicationContext _db;
		private readonly ConfigurationService _configurationService;
		public DbMainController(ApplicationContext db, ConfigurationService configurationService)
		{
			_db = db;
			_configurationService = configurationService;
		}
		public void DeleteDb()
		{
			Log.Error(_configurationService.activeUser.login + "удаляет базу данных");
			_db.dbAccessSemaphore.Wait();
			_db.Database.EnsureDeleted();
			_db.SaveChanges();
			_db.dbAccessSemaphore.Release();
		}
		public void CreateDb()
		{
			Log.Error(_configurationService.activeUser.login + "создает НОВУЮ базу данных");
			_db.dbAccessSemaphore.Wait();
			_db.Database.EnsureCreated();
			_db.SaveChanges();
			_db.dbAccessSemaphore.Release();
		}
		public bool CheckDbExist()
		{
			_db.dbAccessSemaphore.Wait();
			bool response = _db.Database.CanConnect();
			_db.dbAccessSemaphore.Release();
			return response;
		}
	}
}
