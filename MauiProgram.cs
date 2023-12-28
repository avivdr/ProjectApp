using Microsoft.Extensions.Logging;
using ProjectApp.ViewModel;
using ProjectApp.View;
using CommunityToolkit.Maui;
using ProjectApp.Services;
using Syncfusion.Maui.Core.Hosting;

namespace ProjectApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf0x0Qnxbf1x0ZFZMZFVbQHRPMyBoS35RdURhW3dedHVWRWlfWUBx");

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
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
        builder.Services.AddSingleton<Debounce>();
        builder.Services.AddSingleton<DebounceViewModel>();
		builder.Services.AddSingleton<UploadPost>();
		builder.Services.AddSingleton<UploadPostViewModel>();

		builder.Services.AddSingleton<Service>();
		
		builder.ConfigureSyncfusionCore();

        return builder.Build();
	}
}
