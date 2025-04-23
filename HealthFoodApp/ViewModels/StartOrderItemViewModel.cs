using HealthFoodApp.Models;
using HealthFoodApp.Services;
using HealthFoodApp.ViewModels;
using System.Windows.Input;
namespace HealthFoodApp.ViewModels 
{

    [QueryProperty(nameof(Id), "RestaurantId")]
    public class StartOrderItemViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        public ICommand AddToCartCommand { get; }

        private Restaurant _restaurant;
        public Restaurant Restaurant
        {
            get => _restaurant;
            set
            {
                if (SetProperty(ref _restaurant, value))
                {
                    OnPropertyChanged(nameof(PopularItems));
                }
            }
        }
        public List<PopularItem> PopularItems => Restaurant?.PopularItems ?? new List<PopularItem>();

        public StartOrderItemViewModel(DataService dataService)
        {
            _dataService = dataService;
            AddToCartCommand = new Command<Restaurant>(OnAddToCart);
        }

        public StartOrderItemViewModel() : this(DataService.Instance) { }

        private void OnAddToCart(Restaurant restaurant)
        {
            // xử lý thêm món vào giỏ hàng
        }

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                if (SetProperty(ref _id, value))
                {
                    LoadRestaurantDetailsAsync(value);
                }
            }
        }

        private async void LoadRestaurantDetailsAsync(string id)
        {
            Console.WriteLine($"🔍 LoadRestaurantDetailsAsync called with id: {id}");
            var result = await _dataService.GetRestaurantByIdAsync(id);
            if (result != null)
            {
                Restaurant = result;
            }
        }
    }
}
