using HealthFoodApp.Models;
using HealthFoodApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthFoodApp.ViewModels;

public class AccountViewModel : BaseViewModel
{
    private UserProfile _userProfile;
    private bool _isLoggedIn;

    public UserProfile UserProfile
    {
        get => _userProfile;
        set => SetProperty(ref _userProfile, value);
    }

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set => SetProperty(ref _isLoggedIn, value);
    }

    public ObservableCollection<HealthAchievement> HealthAchievements { get; } = new ObservableCollection<HealthAchievement>();
    public ObservableCollection<OrderHistoryItem> OrderHistory { get; } = new ObservableCollection<OrderHistoryItem>();
    public ObservableCollection<Restaurant> FavoriteRestaurants { get; } = new ObservableCollection<Restaurant>();

    public ICommand EditProfileCommand { get; }
    public ICommand ViewHealthProfileCommand { get; }
    public ICommand ViewOrderDetailsCommand { get; }
    public ICommand ViewRestaurantCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand LoginCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand PaymentMethodsCommand { get; }
    public ICommand AddressesCommand { get; }
    public ICommand HelpCommand { get; }

    public AccountViewModel()
    {
        Title = "Tài khoản";

        // Khởi tạo commands
        EditProfileCommand = new Command(EditProfile);
        ViewHealthProfileCommand = new Command(ViewHealthProfile);
        ViewOrderDetailsCommand = new Command<OrderHistoryItem>(ViewOrderDetails);
        ViewRestaurantCommand = new Command<Restaurant>(ViewRestaurant);
        LogoutCommand = new Command(Logout);
        LoginCommand = new Command(Login);
        SettingsCommand = new Command(OpenSettings);
        PaymentMethodsCommand = new Command(OpenPaymentMethods);
        AddressesCommand = new Command(OpenAddresses);
        HelpCommand = new Command(OpenHelp);

        // Kiểm tra xem người dùng đã đăng nhập chưa
        IsLoggedIn = Preferences.Get("IsLoggedIn", false);

        // Đăng ký nhận thông báo đăng nhập thành công
        MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccessful", (sender) =>
        {
            IsLoggedIn = true;
            // Khởi tạo dữ liệu mẫu
            InitializeSampleData();
        });

        // Khởi tạo dữ liệu mẫu nếu đã đăng nhập
        if (IsLoggedIn)
        {
            InitializeSampleData();
        }
    }

    private void InitializeSampleData()
    {
        // Lấy email đã đăng nhập
        string userEmail = Preferences.Get("UserEmail", "user@example.com");

        // Hồ sơ người dùng mẫu
        UserProfile = new UserProfile
        {
            Name = "Nguyễn Văn A",
            Email = userEmail,
            Phone = "0912345678",
            ProfileImage = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?q=80&w=200&h=200&auto=format&fit=crop",
            MemberSince = "Tháng 3, 2023",
            HealthProfile = new HealthProfile
            {
                ActivityLevel = "Vận động vừa phải",
                SleepPattern = "Thức khuya",
                EnergyLevel = "Năng lượng cao"
            }
        };

        // Xóa dữ liệu cũ
        HealthAchievements.Clear();
        OrderHistory.Clear();
        FavoriteRestaurants.Clear();

        // Thành tựu sức khỏe mẫu
        HealthAchievements.Add(new HealthAchievement { Title = "Điểm ngủ 75%+ trong 3 ngày", Icon = "sleep.png", Date = "15/04/2023", Description = "Duy trì chất lượng giấc ngủ tốt trong 3 ngày liên tiếp" });
        HealthAchievements.Add(new HealthAchievement { Title = "Chuỗi hoạt động: 5 ngày", Icon = "activity.png", Date = "10/04/2023", Description = "Hoạt động thể chất đều đặn trong 5 ngày liên tiếp" });
        HealthAchievements.Add(new HealthAchievement { Title = "Mức độ căng thẳng 20% hoặc thấp hơn trong tuần này", Icon = "stress.png", Date = "05/04/2023", Description = "Duy trì mức độ căng thẳng thấp trong cả tuần" });

        // Lịch sử đơn hàng mẫu
        OrderHistory.Add(new OrderHistoryItem
        {
            OrderId = "ORD-12345",
            Date = "20/04/2023",
            Restaurant = "Green Kitchen",
            Items = new List<string> { "Protein-Rich Bowl", "Hydrating Smoothie" },
            Total = "249.000đ",
            Status = "Đã giao",
            StatusColor = "#4CAF50"
        });
        OrderHistory.Add(new OrderHistoryItem
        {
            OrderId = "ORD-12344",
            Date = "18/04/2023",
            Restaurant = "Nourish Cafe",
            Items = new List<string> { "Balanced Lunch", "Evening Calm Tea" },
            Total = "299.000đ",
            Status = "Đã giao",
            StatusColor = "#4CAF50"
        });
        OrderHistory.Add(new OrderHistoryItem
        {
            OrderId = "ORD-12343",
            Date = "15/04/2023",
            Restaurant = "Fuel Kitchen",
            Items = new List<string> { "Energy Boost", "Protein Shake" },
            Total = "179.000đ",
            Status = "Đã hủy",
            StatusColor = "#F44336"
        });

        // Nhà hàng yêu thích mẫu
        var dataService = Services.DataService.Instance;
        foreach (var restaurant in dataService.Restaurants.Take(3))
        {
            FavoriteRestaurants.Add(restaurant);
        }
        // Nhà hàng yêu thích mẫu
        var dataServiceLimit = Services.DataService.Instance;
        foreach (var restaurant in dataService.RestaurantLimits.Take(3))
        {
            FavoriteRestaurants.Add(restaurant);
        }
    }

    private void EditProfile()
    {
        // Điều hướng đến trang chỉnh sửa hồ sơ
        Shell.Current.DisplayAlert("Thông báo", "Chức năng đang được phát triển", "OK");
    }

    private void ViewHealthProfile()
    {
        // Điều hướng đến trang hồ sơ sức khỏe
        Shell.Current.DisplayAlert("Thông báo", "Chức năng đang được phát triển", "OK");
    }

    private void ViewOrderDetails(OrderHistoryItem order)
    {
        // Điều hướng đến trang chi tiết đơn hàng
        Shell.Current.DisplayAlert("Chi tiết đơn hàng", $"Đơn hàng: {order.OrderId}\nNgày: {order.Date}\nNhà hàng: {order.Restaurant}\nTổng: {order.Total}", "OK");
    }

    private async void ViewRestaurant(Restaurant restaurant)
    {
        // Điều hướng đến trang chi tiết nhà hàng
        var parameters = new Dictionary<string, object>
        {
            { "Restaurant", restaurant }
        };
        await Shell.Current.GoToAsync($"{nameof(RestaurantDetailPage)}", parameters);
    }

    private void Logout()
    {
        // Xác nhận đăng xuất
        Device.BeginInvokeOnMainThread(async () =>
        {
            bool confirm = await Shell.Current.DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất", "Hủy");

            if (confirm)
            {
                // Xóa dữ liệu người dùng
                HealthAchievements.Clear();
                OrderHistory.Clear();
                FavoriteRestaurants.Clear();

                // Xóa trạng thái đăng nhập
                Preferences.Remove("IsLoggedIn");
                Preferences.Remove("UserEmail");

                // Đặt IsLoggedIn = false sau khi đã xóa dữ liệu
                IsLoggedIn = false;

                await Shell.Current.DisplayAlert("Đăng xuất", "Bạn đã đăng xuất thành công", "OK");
            }
        });
    }

    private async void Login()
    {
        // Điều hướng đến trang đăng nhập
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private void OpenSettings()
    {
        Shell.Current.DisplayAlert("Cài đặt", "Chức năng đang được phát triển", "OK");
    }

    private void OpenPaymentMethods()
    {
        Shell.Current.DisplayAlert("Phương thức thanh toán", "Chức năng đang được phát triển", "OK");
    }

    private void OpenAddresses()
    {
        Shell.Current.DisplayAlert("Địa chỉ", "Chức năng đang được phát triển", "OK");
    }

    private void OpenHelp()
    {
        Shell.Current.DisplayAlert("Trợ giúp", "Chức năng đang được phát triển", "OK");
    }
}
