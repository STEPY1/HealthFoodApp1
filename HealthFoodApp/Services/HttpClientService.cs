namespace HealthFoodApp
{
    public partial class HttpClientService
    {
        public HttpClient GetPlatfromSpecificHttpClient()
        {
            var Httphandler = GetPlatformSpecificHttpMessageHandler();
            return new HttpClient(Httphandler);
        }
        // Provide an implementation for the partial method
        private partial HttpMessageHandler GetPlatformSpecificHttpMessageHandler();
    }
}
