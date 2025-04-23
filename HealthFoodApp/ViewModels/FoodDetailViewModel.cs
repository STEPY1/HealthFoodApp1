using HealthFoodApp.Models;

namespace HealthFoodApp.ViewModels;

[QueryProperty(nameof(FoodItem), "FoodItem")]
public class FoodDetailViewModel : BaseViewModel
{
    private FoodItem _foodItem;
    public FoodItem FoodItem
    {
        get => _foodItem;
        set => SetProperty(ref _foodItem, value);
    }
}
