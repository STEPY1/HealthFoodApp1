using HealthFoodApp.Models;
using HealthFoodApp.Views;
using System.Windows.Input;

namespace HealthFoodApp.ViewModels;

[QueryProperty(nameof(Restaurant), "Restaurant")]
public class RestaurantDetailViewModel : BaseViewModel
{
    private Restaurant? _restaurant;
    public Restaurant? Restaurant
    {
        get => _restaurant;
        set => SetProperty(ref _restaurant, value);
    }
    public ICommand commandBack { get; }
    public RestaurantDetailViewModel()
    {
        commandBack = new Command<string>(async (restaurantid) =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "RestaurantId", restaurantid }
            };

            await Shell.Current.GoToAsync($"{nameof(StartOrderPage)}?RestaurantId={restaurantid}");
        });
    }
}
