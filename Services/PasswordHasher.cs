using System;
using System.Security.Cryptography;
using System.Text;

namespace CameraDiplomat.Services
{
	public class PasswordHasher
	{
		private const int SaltSize = 16;
		private const int HashSize = 32;
		private const int Iterations = 10000;

		public static string HashPassword(string passwordToHash)
		{
			try
			{
				if (String.IsNullOrEmpty(passwordToHash))
				{
					return "error";
				}
				byte[] salt;
				new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
				var pbkdf2 = new Rfc2898DeriveBytes(passwordToHash, salt, Iterations);
				byte[] hash = pbkdf2.GetBytes(HashSize);
				byte[] hashBytes = new byte[SaltSize + HashSize];
				Array.Copy(salt, 0, hashBytes, 0, SaltSize);
				Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
				return Convert.ToBase64String(hashBytes);
			}
			catch (Exception ex)
			{
				return "error";
			}
		}
		public static bool VerificatePassword(string password, string hashPasswordFromDb)
		{
			try
			{
				if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(hashPasswordFromDb))
				{
					return false;
				}

				if (String.Equals(password, "error") || String.Equals(hashPasswordFromDb, "error"))
				{
					return false;
				}

				byte[] hashBytes = Convert.FromBase64String(hashPasswordFromDb);

				// Извлекаем соль
				byte[] salt = new byte[SaltSize];
				Array.Copy(hashBytes, 0, salt, 0, SaltSize);

				// Создаем хеш для введенного пароля с использованием соли
				var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
				byte[] computedHash = pbkdf2.GetBytes(HashSize);

				// Сравниваем полученный хеш с сохраненным
				for (int i = 0; i < HashSize; i++)
				{
					if (hashBytes[i + SaltSize] != computedHash[i])
						return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	}
}
