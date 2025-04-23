namespace HealthFoodApp
{
    public partial class HttpClientService
    {
        private partial HttpMessageHandler GetPlatformSpecificHttpMessageHandler()
        {
            var handler = new NSUrlSessionHandler();
            return handler;
        }
    }
}
