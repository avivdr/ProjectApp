using Microsoft.Extensions.Logging;
using ProjectApp.ViewModel;
using ProjectApp.View;
using CommunityToolkit.Maui;
using ProjectApp.Services;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui.Core;

namespace ProjectApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit().UseMauiCommunityToolkitMediaElement()
			.ConfigureSyncfusionCore()
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
        builder.Services.AddSingleton<SignUp>();
        builder.Services.AddSingleton<SignUpViewModel>();
        builder.Services.AddTransientPopup<Login, LoginViewModel>();
		builder.Services.AddSingleton<UploadPost>();
		builder.Services.AddSingleton<UploadPostViewModel>();
        builder.Services.AddTransient<ViewPost>();
        builder.Services.AddTransient<ViewPostViewModel>();
        builder.Services.AddSingleton<Logout>();
        builder.Services.AddSingleton<LogoutViewModel>();

        builder.Services.AddSingleton<Service>();
		builder.Services.AddSingleton<UserService>();

        return builder.Build();
	}
}
