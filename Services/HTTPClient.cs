using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TopKebab.Services
{
    public static class HTTPClient
    {
        public static HttpClient sharedClient = new HttpClient()
        {
            BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/nearbysearch/json"),
        };

        internal static void WriteRequestToConsole(this HttpResponseMessage response)
        {
            if (response is null)
            {
                return;
            }

            var request = response.RequestMessage;
            Console.Write($"{request?.Method} ");
            Console.Write($"{request?.RequestUri} ");
            Console.WriteLine($"HTTP/{request?.Version}");
        }
    }

}
