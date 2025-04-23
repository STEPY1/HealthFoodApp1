using HealthFoodApp.ViewModels;

namespace HealthFoodApp.Views;

public partial class ViewAPiHttps : ContentPage
{
    public ViewAPiHttps()
    {
        InitializeComponent();
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as testmodelapi;
        if (viewModel != null)
        {
            await viewModel.LoadCategoriesAsync();
        }
    }
    bool isMenuOpen = false;

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (!isMenuOpen)
        {
            SideMenu.IsVisible = true;

            // Ẩn các nút và đặt vị trí bắt đầu
            BtnHome.Opacity = 0;
            BtnSettings.Opacity = 0;
            BtnLogout.Opacity = 0;

            BtnHome.TranslationX = -50;
            BtnSettings.TranslationX = -50;
            BtnLogout.TranslationX = -50;

            // Animate hiển thị SideMenu và thu nhỏ MainContent
            await Task.WhenAll(
                SideMenu.TranslateTo(0, 0, 250, Easing.SinIn),
                MainContent.TranslateTo(250, 0, 250, Easing.SinIn),
                MainContent.ScaleTo(0.7, 250, Easing.SinIn)
            );

            // Animate từng nút lần lượt
            await Task.WhenAll(
                BtnHome.TranslateTo(0, 0, 150, Easing.SinOut),
                BtnHome.FadeTo(1, 150)
            );
            await Task.Delay(10);

            await Task.WhenAll(
                BtnSettings.TranslateTo(0, 0, 150, Easing.SinOut),
                BtnSettings.FadeTo(1, 150)
            );
            await Task.Delay(10);

            await Task.WhenAll(
                BtnLogout.TranslateTo(0, 0, 150, Easing.SinOut),
                BtnLogout.FadeTo(1, 150)
            );
        }
        else
        {
            await Task.WhenAll(
                SideMenu.TranslateTo(-250, 0, 250, Easing.SinOut),
                MainContent.TranslateTo(0, 0, 250, Easing.SinOut),
                MainContent.ScaleTo(1, 250, Easing.SinOut)
            );

            SideMenu.IsVisible = false;
        }

        isMenuOpen = !isMenuOpen;
    }

    private async void CloseMenu()
    {
        await Task.WhenAll(
            SideMenu.TranslateTo(-250, 0, 250, Easing.SinOut),
            MainContent.TranslateTo(0, 0, 250, Easing.SinOut)
        );
        SideMenu.IsVisible = false;
        isMenuOpen = false;
    }
    double panX = 0;

}