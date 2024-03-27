using CameraDiplomat.Entities;
using CameraDiplomat.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Context
{
	public class ApplicationContext : DbContext
	{

		public DbSet<User> Users { get; set; }
		public DbSet<ProductDataDecoded> Products { get; set; }

		private string _pathToDb;
	

		public ApplicationContext()
		{
			//Взять с сервиса
			_pathToDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CamerasDimplomat.db");
		}

		

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={_pathToDb}");
		}

	}
}
