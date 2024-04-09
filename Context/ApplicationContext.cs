using CameraDiplomat.Entities;
using CameraDiplomat.Services;
using Microsoft.EntityFrameworkCore;

namespace CameraDiplomat.Context
{
	public class ApplicationContext : DbContext
	{
		private readonly ConfigurationService _configurationService;
		public SemaphoreSlim dbAccessSemaphore;
		public DbSet<User> Users { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Session> Sessions { get; set; }

		public ApplicationContext(ConfigurationService configurationService)
		{
			_configurationService = configurationService;
			dbAccessSemaphore = new SemaphoreSlim(1,1);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlite($"Data Source={_configurationService.PathToDb}");
		}

	}
}
