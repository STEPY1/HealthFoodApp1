using HealthFoodApp.Models;
using HealthFoodApp.Services;
using HealthFoodApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthFoodApp.ViewModels;

public class SearchViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private string _searchQuery;
    private bool _isLoading;
    private bool _isSearchResultsVisible;
    private bool _isRecentSearchesVisible = true;
    private bool _noResults;
    private bool _hasActiveFilters;

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
            {
                OnPropertyChanged(nameof(IsSearchResultsVisible));
                OnPropertyChanged(nameof(IsRecentSearchesVisible));
            }
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool IsSearchResultsVisible
    {
        get => _isSearchResultsVisible;
        set => SetProperty(ref _isSearchResultsVisible, value);
    }

    public bool IsRecentSearchesVisible
    {
        get => _isRecentSearchesVisible;
        set => SetProperty(ref _isRecentSearchesVisible, value);
    }

    public bool NoResults
    {
        get => _noResults;
        set => SetProperty(ref _noResults, value);
    }

    public bool HasActiveFilters
    {
        get => _hasActiveFilters;
        set => SetProperty(ref _hasActiveFilters, value);
    }

    public bool HasFoodResults => FoodResults.Count > 0;
    public bool HasRestaurantResults => RestaurantResults.Count > 0;

    public ObservableCollection<string> RecentSearches { get; } = new ObservableCollection<string>();
    public ObservableCollection<string> PopularSearches { get; } = new ObservableCollection<string>();
    public ObservableCollection<FoodItem> FoodResults { get; } = new ObservableCollection<FoodItem>();
    public ObservableCollection<Restaurant> RestaurantResults { get; } = new ObservableCollection<Restaurant>();
    public ObservableCollection<Filter> ActiveFilters { get; } = new ObservableCollection<Filter>();

    public ICommand SearchCommand { get; }
    public ICommand ClearRecentSearchesCommand { get; }
    public ICommand UseRecentSearchCommand { get; }
    public ICommand ShowFiltersCommand { get; }
    public ICommand RemoveFilterCommand { get; }
    public ICommand FoodItemTappedCommand { get; }
    public ICommand RestaurantTappedCommand { get; }

    public SearchViewModel()
    {
        _dataService = DataService.Instance;
        Title = "Tìm kiếm";

        SearchCommand = new Command(ExecuteSearch);
        ClearRecentSearchesCommand = new Command(ClearRecentSearches);
        UseRecentSearchCommand = new Command<string>(UseRecentSearch);
        ShowFiltersCommand = new Command(ShowFilters);
        RemoveFilterCommand = new Command<Filter>(RemoveFilter);
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

        // Initialize with sample data
        PopulatePopularSearches();
    }

    public void Initialize()
    {
        LoadRecentSearches();
    }

    private void LoadRecentSearches()
    {
        // In a real app, this would load from local storage or a service
        if (RecentSearches.Count == 0)
        {
            RecentSearches.Add("Salad");
            RecentSearches.Add("Protein Bowl");
            RecentSearches.Add("Smoothie");
        }
    }

    private void PopulatePopularSearches()
    {
        PopularSearches.Clear();
        PopularSearches.Add("Healthy");
        PopularSearches.Add("Protein");
        PopularSearches.Add("Vegan");
        PopularSearches.Add("Gluten-Free");
        PopularSearches.Add("Low Carb");
        PopularSearches.Add("High Protein");
        PopularSearches.Add("Organic");
        PopularSearches.Add("Keto");
    }

    public void HandleSearchTextChanged(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            IsSearchResultsVisible = false;
            IsRecentSearchesVisible = true;
            return;
        }

        // If the user is typing, we might want to show suggestions
        // but we'll wait for them to press search to actually search
    }

    private async void ExecuteSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            IsSearchResultsVisible = false;
            IsRecentSearchesVisible = true;
            return;
        }

        IsLoading = true;
        IsSearchResultsVisible = true;
        IsRecentSearchesVisible = false;

        // Clear previous results
        FoodResults.Clear();
        RestaurantResults.Clear();

        // Simulate network delay
        await Task.Delay(1000);

        // Perform search (in a real app, this would call an API)
        var query = SearchQuery.ToLowerInvariant();
        var foodItems = _dataService.HealthFoodItems.Concat(_dataService.RegularFoodItems)
            .Where(f => f.Title.ToLowerInvariant().Contains(query) ||
                        f.Restaurant.ToLowerInvariant().Contains(query) ||
                        (f.HealthTag != null && f.HealthTag.ToLowerInvariant().Contains(query)))
            .ToList();

        var restaurants = _dataService.Restaurants
            .Where(r => r.Name.ToLowerInvariant().Contains(query) ||
                        r.Tags.Any(t => t.ToLowerInvariant().Contains(query)))
            .ToList();

        // Update results
        foreach (var food in foodItems)
        {
            FoodResults.Add(food);
        }

        foreach (var restaurant in restaurants)
        {
            RestaurantResults.Add(restaurant);
        }

        // Update UI state
        NoResults = !FoodResults.Any() && !RestaurantResults.Any();
        OnPropertyChanged(nameof(HasFoodResults));
        OnPropertyChanged(nameof(HasRestaurantResults));

        // Add to recent searches if not already there
        if (!RecentSearches.Contains(SearchQuery) && !NoResults)
        {
            RecentSearches.Insert(0, SearchQuery);
            if (RecentSearches.Count > 5)
            {
                RecentSearches.RemoveAt(RecentSearches.Count - 1);
            }
        }

        IsLoading = false;
    }

    private void ClearRecentSearches()
    {
        RecentSearches.Clear();
    }

    private void UseRecentSearch(string searchTerm)
    {
        SearchQuery = searchTerm;
        ExecuteSearch();
    }

    private async void ShowFilters()
    {
        // In a real app, this would show a filter dialog or navigate to a filter page
        // For now, we'll just add some sample filters
        await Task.Delay(500);

        if (!HasActiveFilters)
        {
            ActiveFilters.Add(new Filter { Id = 1, Name = "Healthy" });
            ActiveFilters.Add(new Filter { Id = 2, Name = "Under $15" });
            HasActiveFilters = true;
        }
        else
        {
            ActiveFilters.Clear();
            HasActiveFilters = false;
        }

        // Re-run search with filters
        if (IsSearchResultsVisible)
        {
            ExecuteSearch();
        }
    }

    private void RemoveFilter(Filter filter)
    {
        ActiveFilters.Remove(filter);
        HasActiveFilters = ActiveFilters.Count > 0;

        // Re-run search with updated filters
        if (IsSearchResultsVisible)
        {
            ExecuteSearch();
        }
    }
}

public class Filter
{
    public int Id { get; set; }
    public string Name { get; set; }
}
