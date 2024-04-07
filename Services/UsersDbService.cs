using CameraDiplomat.Context;
using CameraDiplomat.Entities;

namespace CameraDiplomat.Services
{
	public class UsersDbService
	{
		private ConfigurationService _configurationService;
		ApplicationContext _db = new ApplicationContext();
		public UsersDbService(ConfigurationService configurationService, ApplicationContext db)
		{
			_configurationService = configurationService;
			_db = db;
		}

		public async Task<bool> CreateUserAsync(User newUser)
		{
			string messageFromHaser = PasswordHasher.HashPassword(newUser.password);
			if (!String.Equals(messageFromHaser, "error"))
			{
				newUser.password = messageFromHaser;
				_db.Users.Add(newUser);
				await _db.SaveChangesAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<string> EditUserAsync(User oldUser, User newUser)
		{
			try
			{
				var response = await _db.Users.FindAsync(oldUser.id);
				if (response == null)
				{
					return "selected user not found!";
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
				return "success";
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}
		}

		public async Task DeleteUserAsync(User userToDelete)
		{
			_db.Users.Remove(userToDelete);
			await _db.SaveChangesAsync();
		}

		public List<User> GetUsers()
		{
			return _db.Users.ToList();
		}
		public string Authentificate(string _login, string _password)
		{
			User userWhoTryEnter = _db.Users.FirstOrDefault(l => l.login == _login);

			//_password = PasswordHasher.HashPassword(_password);

			if (userWhoTryEnter != null)
			{
				//userWhoTryEnter.password == _password
				if (PasswordHasher.VerificatePassword(_password, userWhoTryEnter.password))
				{
					_configurationService.activeUser = userWhoTryEnter;
					return "success";
				}
				else
				{
					return "error";
				}
			}
			else
			{
				return "error";
			}
		}
	}
}
