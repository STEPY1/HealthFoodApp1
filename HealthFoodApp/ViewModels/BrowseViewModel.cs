using HealthFoodApp.Models;
using HealthFoodApp.Services;
using HealthFoodApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthFoodApp.ViewModels;

public class BrowseViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private string _selectedCategory;

    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (SetProperty(ref _selectedCategory, value))
            {
                FilterRestaurantsByCategory();
            }
        }
    }

    public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
    public ObservableCollection<Restaurant> FeaturedRestaurants { get; } = new ObservableCollection<Restaurant>();
    public ObservableCollection<Restaurant> FilteredRestaurants { get; } = new ObservableCollection<Restaurant>();
    public ObservableCollection<CuisineType> CuisineTypes { get; } = new ObservableCollection<CuisineType>();
    public ObservableCollection<SpecialOffer> SpecialOffers { get; } = new ObservableCollection<SpecialOffer>();

    public ICommand CategorySelectedCommand { get; }
    public ICommand RestaurantTappedCommand { get; }
    public ICommand CuisineSelectedCommand { get; }
    public ICommand OfferSelectedCommand { get; }
    public ICommand SortCommand { get; }

    public BrowseViewModel()
    {
        _dataService = DataService.Instance;
        Title = "Duyệt";
        _selectedCategory = "Tất cả";

        // Initialize commands
        CategorySelectedCommand = new Command<string>(SelectCategory);
        RestaurantTappedCommand = new Command<Restaurant>(async (restaurant) =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "Restaurant", restaurant }
            };
            await Shell.Current.GoToAsync($"{nameof(RestaurantDetailPage)}", parameters);
        });
        CuisineSelectedCommand = new Command<CuisineType>(SelectCuisine);
        OfferSelectedCommand = new Command<SpecialOffer>(SelectOffer);
        SortCommand = new Command<string>(SortRestaurants);

        // Initialize with sample data
        InitializeSampleData();
    }

    private void InitializeSampleData()
    {
        // Add "All" category
        Categories.Add(new Category { Id = "0", Title = "Tất cả", Image = "all.png", Count = "Tất cả" });

        // Add categories from data service
        foreach (var category in _dataService.Categories)
        {
            Categories.Add(category);
        }

        // Add cuisine types
        CuisineTypes.Add(new CuisineType { Id = 1, Name = "Việt Nam", Image = "https://images.unsplash.com/photo-1503764654157-72d979d9af2f?q=80&w=80&h=80&auto=format&fit=crop" });
        CuisineTypes.Add(new CuisineType { Id = 2, Name = "Nhật Bản", Image = "https://images.unsplash.com/photo-1611143669185-af224c5e3252?q=80&w=80&h=80&auto=format&fit=crop" });
        CuisineTypes.Add(new CuisineType { Id = 3, Name = "Ý", Image = "https://images.unsplash.com/photo-1595295333158-4742f28fbd85?q=80&w=80&h=80&auto=format&fit=crop" });
        CuisineTypes.Add(new CuisineType { Id = 4, Name = "Thái", Image = "https://images.unsplash.com/photo-1559847844-5315695dadae?q=80&w=80&h=80&auto=format&fit=crop" });
        CuisineTypes.Add(new CuisineType { Id = 5, Name = "Ấn Độ", Image = "https://images.unsplash.com/photo-1585937421612-70a008356c36?q=80&w=80&h=80&auto=format&fit=crop" });

        // Add special offers
        SpecialOffers.Add(new SpecialOffer
        {
            Id = 1,
            Title = "Giảm 30% cho đơn hàng đầu tiên",
            Description = "Áp dụng cho tất cả nhà hàng",
            Code = "WELCOME30",
            ExpiryDate = "30/05/2023",
            BackgroundColor = "#4CAF50",
            Image = "https://images.unsplash.com/photo-1504674900247-0877df9cc836?q=80&w=320&h=160&auto=format&fit=crop"
        });
        SpecialOffers.Add(new SpecialOffer
        {
            Id = 2,
            Title = "Miễn phí giao hàng",
            Description = "Cho đơn hàng từ 200.000đ",
            Code = "FREESHIP",
            ExpiryDate = "15/05/2023",
            BackgroundColor = "#2196F3",
            Image = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?q=80&w=320&h=160&auto=format&fit=crop"
        });
        SpecialOffers.Add(new SpecialOffer
        {
            Id = 3,
            Title = "Giảm 50.000đ",
            Description = "Cho đơn hàng từ 300.000đ",
            Code = "SAVE50K",
            ExpiryDate = "20/05/2023",
            BackgroundColor = "#FF9800",
            Image = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?q=80&w=320&h=160&auto=format&fit=crop"
        });

        // Add featured restaurants
        foreach (var restaurant in _dataService.Restaurants
             .Where(r => double.TryParse(r.Rating, out var rating) && rating >= 4.5)
             .Take(3))
        {
            FeaturedRestaurants.Add(restaurant);
        }

        // Initialize filtered restaurants with all restaurants
        FilterRestaurantsByCategory();
    }

    private void SelectCategory(string category)
    {
        SelectedCategory = category;
    }

    private void FilterRestaurantsByCategory()
    {
        FilteredRestaurants.Clear();

        var restaurants = _dataService.Restaurants;

        if (SelectedCategory != "Tất cả")
        {
            // Filter by category
            restaurants = restaurants.Where(r => r.Tags.Any(t => t.Contains(SelectedCategory))).ToList();
        }

        foreach (var restaurant in restaurants)
        {
            FilteredRestaurants.Add(restaurant);
        }
    }

    private void SelectCuisine(CuisineType cuisine)
    {
        // In a real app, this would filter restaurants by cuisine
        Shell.Current.DisplayAlert("Chọn ẩm thực", $"Bạn đã chọn: {cuisine.Name}", "OK");
    }

    private void SelectOffer(SpecialOffer offer)
    {
        // In a real app, this would show offer details or apply the offer
        Shell.Current.DisplayAlert("Ưu đãi đặc biệt", $"Mã: {offer.Code}\nHạn sử dụng: {offer.ExpiryDate}", "OK");
    }

    private void SortRestaurants(string sortOption)
    {
        List<Restaurant> sortedList = new List<Restaurant>(FilteredRestaurants);

        switch (sortOption)
        {
            case "rating":
                sortedList = sortedList.OrderByDescending(r => double.Parse(r.Rating)).ToList();
                break;
            case "deliveryTime":
                sortedList = sortedList.OrderBy(r => r.DeliveryTime).ToList();
                break;
            case "deliveryFee":
                sortedList = sortedList.OrderBy(r => r.DeliveryFee).ToList();
                break;
            default:
                break;
        }

        FilteredRestaurants.Clear();
        foreach (var restaurant in sortedList)
        {
            FilteredRestaurants.Add(restaurant);
        }
    }
}
