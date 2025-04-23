using System.Windows.Input;

namespace HealthFoodApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _email;
    private string _password;
    private bool _isLoading;
    private bool _rememberMe;
    private string _errorMessage;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool RememberMe
    {
        get => _rememberMe;
        set => SetProperty(ref _rememberMe, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ForgotPasswordCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand TogglePasswordVisibilityCommand { get; }

    private bool _isPasswordVisible;
    public bool IsPasswordVisible
    {
        get => _isPasswordVisible;
        set => SetProperty(ref _isPasswordVisible, value);
    }

    public LoginViewModel()
    {
        Title = "Đăng nhập";

        // Khởi tạo giá trị mặc định
        Email = string.Empty;
        Password = string.Empty;
        RememberMe = true;
        IsPasswordVisible = false;

        // Khởi tạo các command
        LoginCommand = new Command(OnLoginClicked);
        RegisterCommand = new Command(OnRegisterClicked);
        ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
        BackCommand = new Command(OnBackClicked);
        TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);

        // Kiểm tra xem có thông tin đăng nhập đã lưu không
        if (Preferences.ContainsKey("SavedEmail"))
        {
            Email = Preferences.Get("SavedEmail", string.Empty);
            RememberMe = true;
        }
    }

    private async void OnLoginClicked()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Vui lòng nhập email và mật khẩu";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            // Mô phỏng gọi API
            await Task.Delay(1500);

            // Cho mục đích demo, chấp nhận bất kỳ email nào với mật khẩu có ít nhất 6 ký tự
            if (Password.Length >= 6 && Email.Contains("@"))
            {
                // Lưu trạng thái đăng nhập
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserEmail", Email);

                // Lưu email nếu người dùng chọn "Ghi nhớ đăng nhập"
                if (RememberMe)
                {
                    Preferences.Set("SavedEmail", Email);
                }
                else
                {
                    Preferences.Remove("SavedEmail");
                }

                // Đóng trang đăng nhập và quay lại trang trước đó
                await Shell.Current.GoToAsync("..");

                // Thông báo cho AccountViewModel biết đăng nhập thành công
                MessagingCenter.Send(this, "LoginSuccessful");
            }
            else
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Lỗi đăng nhập: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void OnRegisterClicked()
    {
        await Shell.Current.DisplayAlert("Đăng ký", "Chức năng đăng ký đang được phát triển", "OK");
    }

    private async void OnForgotPasswordClicked()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current.DisplayAlert("Quên mật khẩu", "Vui lòng nhập email của bạn trước", "OK");
            return;
        }

        await Shell.Current.DisplayAlert("Quên mật khẩu", $"Hướng dẫn đặt lại mật khẩu đã được gửi đến {Email}", "OK");
    }

    private async void OnBackClicked()
    {
        await Shell.Current.GoToAsync("..");
    }

    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }
}
