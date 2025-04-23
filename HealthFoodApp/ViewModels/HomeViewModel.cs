using HealthFoodApp.Models;
using HealthFoodApp.Services;
using HealthFoodApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthFoodApp.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private string _selectedCategory = "All";
    public bool IsLoading { get; set; }
    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    private FoodItem _FoodItem;
    public FoodItem SelectedFoodItem
    {
        get => _FoodItem;
        set
        {
            if (SetProperty(ref _FoodItem, value))
            {
                // Cập nhật UI khi selected food item thay đổi
                OnPropertyChanged(nameof(SelectedFoodItem));
            }
        }
    }
    private List<Category> _categories;
    public List<Category> Categories1
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged();
        }
    }
    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (SetProperty(ref _selectedCategory, value))
            {
                // Filter items when category changes
                FilterItemsByCategory();
                System.Diagnostics.Debug.WriteLine($"Selected category: {value}");
            }
        }
    }

    public ObservableCollection<string> HealthAchievements { get; }
    public ObservableCollection<FoodItem> HealthFoodItems { get; }
    public ObservableCollection<FoodItem> RegularFoodItems { get; }
    public ObservableCollection<FoodItem> DisplayedFoodItems { get; } = new ObservableCollection<FoodItem>();
    public ObservableCollection<Category> Categories { get; }
    public ObservableCollection<Restaurant> Restaurants { get; }
    public ObservableCollection<Restaurant> RestaurantLimits { get; }

    // Cart properties
    private bool _isCartVisible;
    public bool IsCartVisible
    {
        get => _isCartVisible;
        set => SetProperty(ref _isCartVisible, value);
    }

    public ObservableCollection<CartItem> CartItems { get; } = new ObservableCollection<CartItem>();

    public bool HasCartItems => CartItems.Count > 0;

    public string CartItemCount => CartItems.Sum(item => item.Quantity).ToString();

    public string CartItemsDescription => HasCartItems
        ? $"You have {CartItems.Sum(item => item.Quantity)} items in your cart"
        : "Your cart is empty";

    public string Subtotal => $"${CartItems.Sum(item => item.Quantity * ParsePrice(item.Price)):F2}";
    public string DeliveryFee => "$1.99";
    public string Tax => $"${CartItems.Sum(item => item.Quantity * ParsePrice(item.Price) * 0.1):F2}";
    public string Total => $"${CartItems.Sum(item => item.Quantity * ParsePrice(item.Price)) + 1.99 + (CartItems.Sum(item => item.Quantity * ParsePrice(item.Price)) * 0.1):F2}";

    public ICommand RefreshCommand { get; }
    public ICommand FoodItemTappedCommand { get; }
    public ICommand RestaurantTappedCommand { get; }
    public ICommand CategorySelectedCommand { get; }
    public ICommand ShowCartCommand { get; }
    public ICommand HideCartCommand { get; }
    public ICommand IncreaseQuantityCommand { get; }
    public ICommand DecreaseQuantityCommand { get; }
    public ICommand RemoveFromCartCommand { get; }
    public ICommand CheckoutCommand { get; }

    public HomeViewModel()
    {

        RefreshCommand = new Command(async () => await RefreshDataAsync());
        _dataService = DataService.Instance;
        HealthAchievements = new ObservableCollection<string>(_dataService.HealthAchievements ?? new List<string>());
        HealthFoodItems = new ObservableCollection<FoodItem>(_dataService.HealthFoodItems ?? new List<FoodItem>());
        RegularFoodItems = new ObservableCollection<FoodItem>(_dataService.RegularFoodItems ?? new List<FoodItem>());
        Categories = new ObservableCollection<Category>(_dataService.Categories ?? new List<Category>());
        Restaurants = new ObservableCollection<Restaurant>(_dataService.Restaurants ?? new List<Restaurant>());
        RestaurantLimits = new ObservableCollection<Restaurant>(_dataService.RestaurantLimits ?? new List<Restaurant>());
        Task.Run(async () => await RefreshDataAsync());

        FoodItemTappedCommand = new Command<FoodItem>(async (foodItem) =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "FoodItem", foodItem }
            };
            await Shell.Current.GoToAsync($"{nameof(FoodDetailPage)}", parameters);
        });

        RestaurantTappedCommand = new Command<Restaurant>(async (restaurant) =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "Restaurant", restaurant }
            };
            await Shell.Current.GoToAsync($"{nameof(RestaurantDetailPage)}", parameters);
        });
        CategorySelectedCommand = new Command<string>(SelectCategory);

        // Initialize cart commands
        ShowCartCommand = new Command(ShowCart);
        HideCartCommand = new Command(HideCart);
        IncreaseQuantityCommand = new Command<CartItem>(IncreaseQuantity);
        DecreaseQuantityCommand = new Command<CartItem>(DecreaseQuantity);
        RemoveFromCartCommand = new Command<CartItem>(RemoveFromCart);
        CheckoutCommand = new Command(Checkout);
        // Add sample cart items
        InitializeCart();
    }



    public async Task RefreshDataAsync()
    {
        IsRefreshing = true;
        await _dataService.InitializeData();

        // Clear & update HealthFoodItems
        HealthFoodItems.Clear();
        foreach (var item in _dataService.HealthFoodItems ?? new List<FoodItem>())
            HealthFoodItems.Add(item);

        // Clear & update RegularFoodItems
        RegularFoodItems.Clear();
        foreach (var item in _dataService.RegularFoodItems ?? new List<FoodItem>())
            RegularFoodItems.Add(item);

        // Clear & update RegularFoodItems
        Restaurants.Clear();
        foreach (var item in _dataService.Restaurants ?? new List<Restaurant>())
            Restaurants.Add(item);

        // Clear & update RegularFoodItems
        RestaurantLimits.Clear();
        foreach (var item in _dataService.RestaurantLimits ?? new List<Restaurant>())
            RestaurantLimits.Add(item);

        // Update categories
        Categories.Clear();
        foreach (var item in _dataService.Categories ?? new List<Category>())
            Categories.Add(item);

        FilterItemsByCategory();
        IsRefreshing = false;
    }
    private void SelectCategory(string category)
    {
        if (SelectedCategory != category)
        {
            SelectedCategory = category;
        }
    }
    private void UpdateCollection<T>(ObservableCollection<T> target, List<T> source)
    {
        target.Clear();
        foreach (var item in source)
            target.Add(item);
    }
    private void FilterItemsByCategory()
    {
        // Clear current displayed items
        DisplayedFoodItems.Clear();

        // Combine all food items for filtering
        var allItems = new List<FoodItem>();
        allItems.AddRange(HealthFoodItems);
        allItems.AddRange(RegularFoodItems);

        // Filter based on selected category
        List<FoodItem> filteredItems;

        switch (SelectedCategory)
        {
            case "Healthy":
                // Show only health-focused items
                filteredItems = allItems.Where(item => item.IsHealthFocused).ToList();
                break;

            case "Quick":
                // Show items that can be prepared quickly (for example, items with "quick" in the title or description)
                filteredItems = allItems.Where(item =>
                    item.Title.Contains("Smoothie") ||
                    item.Title.Contains("Energy") ||
                    item.Title.Contains("Boost") ||
                    (item.Description != null && item.Description.Contains("quick"))).ToList();
                break;

            case "Offers":
                // Show items with discounts
                filteredItems = allItems.Where(item => !string.IsNullOrEmpty(item.Discount)).ToList();
                break;

            case "All":
            default:
                // Show all items
                filteredItems = allItems;
                break;
        }

        // Add filtered items to the displayed collection
        foreach (var item in filteredItems)
        {
            DisplayedFoodItems.Add(item);
        }

        System.Diagnostics.Debug.WriteLine($"Filtered items: {DisplayedFoodItems.Count} for category: {SelectedCategory}");
    }

    private void InitializeCart()
    {
        CartItems.Add(new CartItem
        {
            Id = 1,
            Image = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?q=80&w=160&h=120&auto=format&fit=crop",
            Title = "Protein-Rich Bowl",
            Restaurant = "Green Kitchen",
            Price = "$12.99",
            Quantity = 1
        });

        CartItems.Add(new CartItem
        {
            Id = 2,
            Image = "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?q=80&w=160&h=120&auto=format&fit=crop",
            Title = "Hydrating Smoothie",
            Restaurant = "Juice Bar",
            Price = "$6.74",
            Quantity = 2
        });

        OnPropertyChanged(nameof(HasCartItems));
        OnPropertyChanged(nameof(CartItemCount));
        OnPropertyChanged(nameof(CartItemsDescription));
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Tax));
        OnPropertyChanged(nameof(Total));
    }

    private void ShowCart()
    {
        IsCartVisible = true;
    }

    private void HideCart()
    {
        IsCartVisible = false;
    }

    private void IncreaseQuantity(CartItem item)
    {
        if (item != null)
        {
            item.Quantity++;
            UpdateCartTotals();
        }
    }

    private void DecreaseQuantity(CartItem item)
    {
        if (item != null && item.Quantity > 1)
        {
            item.Quantity--;
            UpdateCartTotals();
        }
    }

    private void RemoveFromCart(CartItem item)
    {
        if (item != null)
        {
            CartItems.Remove(item);
            UpdateCartTotals();
        }
    }

    private void UpdateCartTotals()
    {
        OnPropertyChanged(nameof(HasCartItems));
        OnPropertyChanged(nameof(CartItemCount));
        OnPropertyChanged(nameof(CartItemsDescription));
        OnPropertyChanged(nameof(Subtotal));
        OnPropertyChanged(nameof(Tax));
        OnPropertyChanged(nameof(Total));
    }

    private async void Checkout()
    {
        IsCartVisible = false;
        await Shell.Current.DisplayAlert("Checkout", "Proceeding to checkout with total: " + Total, "OK");
    }

    private double ParsePrice(string price)
    {
        if (string.IsNullOrEmpty(price))
            return 0;

        // Remove currency symbol and parse the number
        string numericPart = new string(price.Where(c => char.IsDigit(c) || c == '.').ToArray());
        if (double.TryParse(numericPart, out double result))
            return result;
        return 0;
    }
}
