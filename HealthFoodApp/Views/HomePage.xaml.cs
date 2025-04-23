using HealthFoodApp.Services;
using HealthFoodApp.ViewModels;

namespace HealthFoodApp.Views;

public partial class HomePage : ContentPage
{
    private readonly DataService dataService;
    public HomePage(HomeViewModel homeViewModel)
    {
        InitializeComponent();
        BindingContext = homeViewModel;
        dataService = DataService.Instance;
        _ = InitializePageAsync(homeViewModel);
    }

    private async Task InitializePageAsync(HomeViewModel homeViewModel)
    {
        homeViewModel.IsLoading = true;
        await homeViewModel.RefreshDataAsync(); // dùng chung logic refresh
        homeViewModel.IsLoading = false;
    }

    double currentX = 0.9, currentY = 0.9;
    private void FloatingButton_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                UpdateFloatingPosition(e.TotalX, e.TotalY);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                currentX = FloatingButton.TranslationX;
                currentY = FloatingButton.TranslationY;
                break;
        }
    }

    private void UpdateFloatingPosition(double totalX, double totalY)
    {
        // Lấy kích thước màn hình và kích thước nút
        var layout = (AbsoluteLayout)FloatingButton.Parent;
        var layoutWidth = layout.Width;
        var layoutHeight = layout.Height;

        var buttonWidth = FloatingButton.Width;
        var buttonHeight = FloatingButton.Height;

        // Tính vị trí mới
        double newX = currentX + totalX;
        double newY = currentY + totalY;

        // Giới hạn trong phạm vi màn hình
        newX = Math.Max(0, Math.Min(newX, layoutWidth - buttonWidth));
        newY = Math.Max(0, Math.Min(newY, layoutHeight - buttonHeight));

        FloatingButton.TranslationX = newX;
        FloatingButton.TranslationY = newY;
    }
}
