using Microsoft.Extensions.Logging;
using ProjectApp.ViewModel;
using ProjectApp.View;

namespace ProjectApp;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<Login>();
        builder.Services.AddSingleton<LoginViewModel>();
        return builder.Build();
	}
}
