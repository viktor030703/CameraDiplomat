using CameraDiplomat.Context;
using CameraDiplomat.Data;
using CameraDiplomat.Entities;
using CameraDiplomat.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace CameraDiplomat
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif
			//builder.Configuration.GetConnectionString("");
			// Adding configuration file
			var a = Assembly.GetExecutingAssembly();
			using var stream = a.GetManifestResourceStream("CameraDiplomat.AppConfiguretion.json");
			var config = new ConfigurationBuilder()
						.AddJsonStream(stream)
						.Build();
			builder.Configuration.AddConfiguration(config);

			builder.Services.AddSingleton<ConfigurationService>();

			//builder.Services.AddDbContext<ApplicationContext>(options =>
			//{
			//	options.UseSqlite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CamerasDimplomat.db"));
			//});

			builder.Services.AddSingleton<ApplicationContext>();
			builder.Services.AddSingleton<TCPService>();
			builder.Services.AddSingleton<MessageDecoder>();
			builder.Services.AddSingleton<UsersDbService>();
			builder.Services.AddSingleton<ProductsDbService>();
			builder.Services.AddSingleton<PasswordHasher>();
			builder.Services.AddSingleton<SessionCustomDesigner>();
			builder.Services.AddSingleton<SessionsDbService>();
			builder.Services.AddSingleton<StatsService>();
			builder.Services.AddSingleton<DbMainController>();


			return builder.Build();

		}
	}
}
