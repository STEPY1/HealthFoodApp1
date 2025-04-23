using Microsoft.Maui.Controls;

namespace HealthFoodApp.Views;

public partial class FoodDetailPage : ContentPage
{
    public FoodDetailPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        if (RootLayout != null)
        {
            // Thực hiện hiệu ứng mờ dần
            await RootLayout.FadeTo(0, 300, Easing.CubicOut);

            // Sau khi hiệu ứng xong thì quay lại trang trước
            await Shell.Current.GoToAsync("..");

            // Reset opacity về lại 1 nếu trang quay lại từ đây (optional)
            RootLayout.Opacity = 1;
        }
    }

}
