using CameraDiplomat.Context;
using CameraDiplomat.Entities;

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

		public  void CreateUser(User newUser)
		{
			db.Users.Add(newUser);
			db.SaveChanges();
		}

		public void ChangeUser(User oldUser, User newUser)
		{
			db.Users.Find(oldUser);
			if(!String.IsNullOrEmpty(newUser.id))
				oldUser.id = newUser.id;
			if (!String.IsNullOrEmpty(newUser.login))
				oldUser.login = newUser.login;
			if (!String.IsNullOrEmpty(newUser.password))
				oldUser.password = newUser.password;
			if (!String.IsNullOrEmpty(newUser.role))
				oldUser.role = newUser.role;
			if (!String.IsNullOrEmpty(newUser.lastLoginData))
				oldUser.lastLoginData = newUser.lastLoginData;
			db.SaveChanges();

		}

		public void DeleteUser(User userToDelete)
		{
			db.Users.Remove(userToDelete);
			db.SaveChanges();
		}

		public List<User> GetUsers()
		{
			return db.Users.ToList();
		}


		public string Authentificate(string _login, string _password)
		{
			User userWhoTryEnter = db.Users.FirstOrDefault(l => l.login == _login);
			if(userWhoTryEnter != null)
			{
				if(userWhoTryEnter.password == _password)
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
