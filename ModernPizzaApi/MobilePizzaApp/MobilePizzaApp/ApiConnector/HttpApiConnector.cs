using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MobilePizzaApp.ApiConnector
{
    class HttpApiConnector :IDisposable
    {
        private HttpClient Client { get; set; }

        public HttpApiConnector()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient(clientHandler);
        }


        public HttpClient GetClient()
        {
            return Client;
        }
        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
