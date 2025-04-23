using HealthFoodApp.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace HealthFoodApp.ViewModels
{
    public class testmodelapi : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; } = new();

        public async Task LoadCategoriesAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var httpClient = new HttpClientService().GetPlatfromSpecificHttpClient();
                var baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7116" : "https://localhost:7116";

                var response = await httpClient.GetFromJsonAsync<List<Category>>($"{baseUrl}/api/Category");
                if (response != null)
                {
                    Categories.Clear();
                    foreach (var item in response)
                        Categories.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
