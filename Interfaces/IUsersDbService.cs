using CameraDiplomat.Entities;

namespace CameraDiplomat.Interfaces
{
	public interface IUsersDbService
	{
		Task<bool> CreateUserAsync(User newUser);
		Task<bool> EditUserAsync(User oldUser, User newUser);
		Task DeleteUserAsync(User userToDelete);
		List<User> GetUsers();
		bool Authentificate(string _login, string _password);
	}
}
