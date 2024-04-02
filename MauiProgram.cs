using CameraDiplomat.Context;
using CameraDiplomat.Data;
using CameraDiplomat.Entities;
using CameraDiplomat.Services;
using Microsoft.Extensions.Logging;


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

			builder.Services.AddSingleton<ConfigurationService>();
			builder.Services.AddSingleton<ApplicationContext>();
			builder.Services.AddSingleton<TCPService>();
			builder.Services.AddSingleton<MessageDecoder>();
			builder.Services.AddSingleton<UsersDbService>();
			builder.Services.AddSingleton<ProductsDbService>();
			builder.Services.AddSingleton<PasswordHasher>();
			

			UsersDbService serv = new UsersDbService();

			//serv.DropDb();

			serv.CheckDbExist();

			//Entities.User user = new Entities.User()
			//{
			//	id = Guid.NewGuid().ToString(),
			//	login = "user",
			//	password = "user",
			//	role = "user",
			//};

			//serv.CreateUser(user);

			ProductsDbService db = new ProductsDbService();

			List<ProductDataDecoded> ourList= db.GetProducts(); 


			return builder.Build();

		}
	}
}
