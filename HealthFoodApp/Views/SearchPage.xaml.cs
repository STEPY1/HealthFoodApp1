using HealthFoodApp.ViewModels;

namespace HealthFoodApp.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as SearchViewModel)?.Initialize();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
