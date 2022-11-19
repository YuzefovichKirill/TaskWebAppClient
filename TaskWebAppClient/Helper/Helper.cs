using System;
using System.Net.Http;

namespace TaskWebAppClient.Helper
{
    public class FridgeAPI
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:15389");
            return client;
        }
    }
}
