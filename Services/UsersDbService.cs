using CameraDiplomat.Context;
using CameraDiplomat.Entities;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class UsersDbService : IUsersDbService
	{
		private readonly ConfigurationService _configurationService;
		private readonly ApplicationContext _db;
		public UsersDbService(ConfigurationService configurationService, ApplicationContext db)
		{
			_configurationService = configurationService;
			_db = db;
		}

		public async Task<bool> CreateUserAsync(User newUser)
		{
			Log.Information(_configurationService.activeUser.login + " добавляет пользователя " +  newUser.login);
			string messageFromHaser = PasswordHasher.HashPassword(newUser.password);
			if (!String.Equals(messageFromHaser, "error"))
			{
				newUser.password = messageFromHaser;
				await _db.dbAccessSemaphore.WaitAsync();
				_db.Users.Add(newUser);
				await _db.SaveChangesAsync();
				_db.dbAccessSemaphore.Release();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> EditUserAsync(User oldUser, User newUser)
		{
			try
			{
				Log.Information(_configurationService.activeUser.login + " изменяет пользователя " + oldUser.login);
				await _db.dbAccessSemaphore.WaitAsync();
				var response = await _db.Users.FindAsync(oldUser.id);
				if (response == null)
				{
					Log.Warning("Выбранный пользователь не найден в БД");
					return false;
				}

				if (!String.IsNullOrEmpty(newUser.login))
					oldUser.login = newUser.login;
				if (!String.IsNullOrEmpty(newUser.password))
					oldUser.password = PasswordHasher.HashPassword(newUser.password);
				if (!String.IsNullOrEmpty(newUser.role))
					oldUser.role = newUser.role;
				if (!String.IsNullOrEmpty(newUser.lastLoginData))
					oldUser.lastLoginData = newUser.lastLoginData;

				await _db.SaveChangesAsync();
				_db.dbAccessSemaphore.Release();
				return true;
			}
			catch (Exception ex)
			{
				Log.Warning("Исключение во время изменения пользователя: " + ex.Message.ToString());
				_db.dbAccessSemaphore.Release();
				return false;
			}
		}

		public async Task DeleteUserAsync(User userToDelete)
		{
			Log.Information(_configurationService.activeUser.login + " удаляет пользователя " + userToDelete.login);
		
			await _db.dbAccessSemaphore.WaitAsync();
			_db.Users.Remove(userToDelete);
			await _db.SaveChangesAsync();
			_db.dbAccessSemaphore.Release();
		}

		public List<User> GetUsers()
		{
			_db.dbAccessSemaphore.Wait();
			List<User> users = _db.Users.ToList();
			_db.dbAccessSemaphore.Release();
			return users;
		}
		public bool Authentificate(string _login, string _password)
		{
			_db.dbAccessSemaphore.Wait();
			User userWhoTryEnter = _db.Users.FirstOrDefault(l => l.login == _login);
			if (userWhoTryEnter != null)
			{
				if (PasswordHasher.VerificatePassword(_password, userWhoTryEnter.password))
				{
					_configurationService.activeUser = userWhoTryEnter;
					_db.dbAccessSemaphore.Release();
					Log.Information("Аутентификация пройдена");
					
					return true;
				}
				else
				{
					_db.dbAccessSemaphore.Release();
					Log.Information("Аутентификация не пройдена");
					return false;
					
				}
			}
			else
			{
				_db.dbAccessSemaphore.Release();
				Log.Information("Юзер с таким именем не найден");
				return false;
			}
		}
	}
}
