namespace HealthFoodApp
{
    public partial class HttpClientService
    {
        private partial HttpMessageHandler GetPlatformSpecificHttpMessageHandler()
        {
            return new HttpClientHandler(); // Mặc định
        }
    }
}
