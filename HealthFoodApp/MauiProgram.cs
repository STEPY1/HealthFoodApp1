using CommunityToolkit.Maui;
using HealthFoodApp.Converters;
using HealthFoodApp.Services;
using HealthFoodApp.ViewModels;
using HealthFoodApp.Views;
using Microsoft.Extensions.Logging;

namespace HealthFoodApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton(DataService.Instance);

        // Register view models
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<FoodDetailViewModel>();
        builder.Services.AddTransient<RestaurantDetailViewModel>();
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<AccountViewModel>();
        builder.Services.AddTransient<BrowseViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<testmodelapi>();
        builder.Services.AddTransient<StartOrderItemViewModel>();
        // Register pages
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<FoodDetailPage>();
        builder.Services.AddTransient<RestaurantDetailPage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<BrowsePage>();
        builder.Services.AddTransient<AccountPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ViewAPiHttps>();
        builder.Services.AddTransient<StartOrderPage>();

        // Register converters for resources
        builder.Services.AddSingleton<ResourceDictionary>(new ResourceDictionary
        {
            { "StringNotNullOrEmptyConverter", new StringNotNullOrEmptyConverter() },
            { "CollectionNotEmptyConverter", new CollectionNotEmptyConverter() },
            { "PercentToWidthConverter", new PercentToWidthConverter() },
            { "FirstCharConverter", new FirstCharConverter() }
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
