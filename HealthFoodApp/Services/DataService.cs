using HealthFoodApp.Models;
using System.Net.Http.Json;

namespace HealthFoodApp.Services;

public class DataService
{
    private static DataService? _instance;
    public static DataService Instance => _instance ??= new DataService();
    int RestaurentLimit = 5;
    string id;
    public List<string> HealthAchievements { get; private set; }
    public List<FoodItem> HealthFoodItems { get; private set; }
    public List<FoodItem> RegularFoodItems { get; private set; }
    public List<Category> Categories { get; private set; }
    public List<Restaurant> Restaurants { get; private set; }
    public List<Restaurant> RestaurantLimits { get; private set; }
    public List<Restaurant> RestaurentItem { get; private set; }

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private DataService()
    {
        _httpClient = new HttpClientService().GetPlatfromSpecificHttpClient();
        _baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7116" : "https://localhost:7116";
    }

    public async Task InitializeData()
    {
        try
        {
            var allFoodItems = await _httpClient.GetFromJsonAsync<List<FoodItem>>($"{_baseUrl}/api/FoodItem") ?? new();
            HealthFoodItems = allFoodItems.Where(x => x.IsHealthFocused).ToList();
            RegularFoodItems = allFoodItems.Where(x => !x.IsHealthFocused).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load FoodItems: {ex.Message}");
            HealthFoodItems = new List<FoodItem>();
            RegularFoodItems = new List<FoodItem>();
        }

        try
        {
            Restaurants = await _httpClient.GetFromJsonAsync<List<Restaurant>>($"{_baseUrl}/api/Restaurant") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load Restaurants: {ex.Message}");
            Restaurants = new List<Restaurant>();
        }

        try
        {
            RestaurantLimits = await _httpClient.GetFromJsonAsync<List<Restaurant>>($"{_baseUrl}/api/Restaurant/limit/{RestaurentLimit}") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load Restaurants : {ex.Message}");
            RestaurantLimits = new List<Restaurant>();
        }

        try
        {
            Categories = await _httpClient.GetFromJsonAsync<List<Category>>($"{_baseUrl}/api/Category") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load Categories: {ex.Message}");
            Categories = new List<Category>();
        }

        HealthAchievements = new List<string>
        {
            "Sleep Score 75%+ for 3 days",
            "Activity Streak: 5 days",
            "Stress levels 20% or lower this week"
        };
    }
    public FoodItem GetFoodItemById(int id)
    {
        return HealthFoodItems.FirstOrDefault(f => f.Id == id) ?? RegularFoodItems.FirstOrDefault(f => f.Id == id);
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(string id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Restaurant>($"{_baseUrl}/api/Restaurant/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch restaurant by id {id}: {ex.Message}");
            return null;
        }
    }
}
