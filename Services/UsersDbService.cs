using CameraDiplomat.Context;
using CameraDiplomat.Entities;
using Microsoft.EntityFrameworkCore;

namespace CameraDiplomat.Services
{
	//Добавить хэширование паролей!!!!
	public class UsersDbService
	{
		ApplicationContext db = new ApplicationContext();

		public void DropDb()
		{
			db.Database.EnsureDeleted();
			db.SaveChanges();
		}
		public void CheckDbExist()
		{
			db.Database.EnsureCreated();
			db.SaveChanges();
		}

		public async Task<bool> CreateUserAsync(User newUser)
		{
			string messageFromHaser = PasswordHasher.HashPassword(newUser.password);
			if (!String.Equals(messageFromHaser, "error"))
			{
				newUser.password = messageFromHaser;
				db.Users.Add(newUser);
				await db.SaveChangesAsync();
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
				var response = await db.Users.FindAsync(oldUser.id);
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

				await db.SaveChangesAsync();
				return "success";
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}
		}

		public async Task DeleteUserAsync(User userToDelete)
		{
			db.Users.Remove(userToDelete);
			await db.SaveChangesAsync();
		}

		public List<User> GetUsers()
		{
			return db.Users.ToList();
		}


		public string Authentificate(string _login, string _password)
		{
			User userWhoTryEnter = db.Users.FirstOrDefault(l => l.login == _login);

			//_password = PasswordHasher.HashPassword(_password);

			if (userWhoTryEnter != null)
			{
				//userWhoTryEnter.password == _password
				if (PasswordHasher.VerificatePassword(_password, userWhoTryEnter.password))
				{
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
