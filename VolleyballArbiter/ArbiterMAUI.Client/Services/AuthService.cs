using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ArbiterMAUI.Client.Services.Interfaces;

namespace ArbiterMAUI.Client.Services
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient;

        public AuthService()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };
            _httpClient = new HttpClient(handler);
        }

        public async Task<HttpResponseMessage> CheckRefereeKey()
        {
            if (_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
            {
                _httpClient.DefaultRequestHeaders.Remove("ApiKey");  
            }
            _httpClient.DefaultRequestHeaders.Add("ApiKey", App._refereeKey);

            return await _httpClient.GetAsync(AppStrings.Api.BaseUrl + "/refereeCheck");
        }
    }
}
