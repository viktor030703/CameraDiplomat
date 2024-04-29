using CameraDiplomat.Context;
using CameraDiplomat.Services;
using CameraDiplomat.Interfaces;

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
			//var a = Assembly.GetExecutingAssembly();
			//using var stream = a.GetManifestResourceStream("CameraDiplomat.AppConfiguretion.json");
			//var config = new ConfigurationBuilder()
			//			.AddJsonStream(stream)
			//			.Build();
			//builder.Configuration.AddConfiguration(config);

			//builder.Services.AddDbContext<ApplicationContext>(options =>
			//{
			//	options.UseSqlite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CamerasDimplomat.db"));
			//});

			builder.Services.AddSingleton<ConfigurationService>();
			builder.Services.AddSingleton<ILoggerService, LoggerService>();

			builder.Services.AddSingleton<IStatisticService, StatisticService>();
			builder.Services.AddSingleton<ISessionCustomDesigner, SessionCustomDesigner>();

			builder.Services.AddSingleton<ApplicationContext>();

			builder.Services.AddSingleton<ITCPService, TCPService>();
			builder.Services.AddSingleton<ICameraConnectionService, CameraConnectionService>();

			builder.Services.AddSingleton<IDbMainController, DbMainController>();
			builder.Services.AddSingleton<ISessionsDbService, SessionsDbService>();
			builder.Services.AddSingleton<IProductsDbService, ProductsDbService>();
			builder.Services.AddSingleton<IUsersDbService, UsersDbService>();

			builder.Services.AddSingleton<PasswordHasher>();

			builder.Services.AddSingleton<IMessageDecoder, MessageDecoder>();
			builder.Services.AddSingleton<INonStandartMessageHandler, NonStandartMessageHandler>();

			builder.Services.AddTransient<IShutdownTimer, ShutdownTimer>();

			return builder.Build();

		}
	}
}
