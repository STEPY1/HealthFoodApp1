namespace HealthFoodApp
{
    public partial class HttpClientService
    {
        private partial HttpMessageHandler GetPlatformSpecificHttpMessageHandler()
        {
            // Sử dụng NSUrlSessionHandler cho Mac Catalyst
            var handler = new NSUrlSessionHandler();
            return handler;
        }
    }
}
