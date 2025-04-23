using Xamarin.Android.Net;

namespace HealthFoodApp
{
    public partial class HttpClientService
    {
        private partial HttpMessageHandler GetPlatformSpecificHttpMessageHandler()
        {
            var androidHttpHandler = new AndroidMessageHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    // Accept all certificates
                    return true;
                }
            };
            return androidHttpHandler;
        }
    }
}
