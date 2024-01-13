using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.ViewModels
{
    public class BaseViewModel
    {
        protected HttpClient _httpClient;
        protected bool isInitialLoad = true;

        protected async Task<T> GetRequest<T>(string url, int id = -1)
        {
            url = id == -1 ? url : url + $"?id={id}";
            var responseMessage = await _httpClient.GetAsync(url);
            return await ProcessAPIResponse<T>(responseMessage);
        }

        protected async Task<T> PostRequest<T>(string url, object content)
        {
            HttpContent httpContent = PrepareHTTPContent(content);
            var responseMessage = await _httpClient.PostAsync(url, httpContent);
            return await ProcessAPIResponse<T>(responseMessage);
        }

        protected async Task PutRequest(string url, object content)
        {
            HttpContent httpContent = PrepareHTTPContent(content);
            var responseMessage = await _httpClient.PutAsync(url, httpContent);
            await ProcessAPIResponse(responseMessage);
        }

        protected async Task DeleteRequest(string url, int id)
        {
            var responseMessage = await _httpClient.DeleteAsync(url + $"?id={id}");
            await ProcessAPIResponse(responseMessage);
        }

        private async Task<T> ProcessAPIResponse<T>(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong.", "OK");
                throw new Exception();
            }

            var responseContent = responseMessage.Content.ReadFromJsonAsync<TrainingOrganizerResponse<T>>().Result;
            if (responseContent.Messages?.Count > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Join('\n', responseContent.Messages), "OK");
                throw new Exception();
            }

            return responseContent.Content;
        }

        private async Task ProcessAPIResponse(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong.", "OK");
                throw new Exception();
            }

            var responseContent = await responseMessage.Content.ReadFromJsonAsync<TrainingOrganizerResponse<object>>();
            if (responseContent.Messages?.Count > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Join('\n', responseContent.Messages), "OK");
                throw new Exception();
            }
        }

        private HttpContent PrepareHTTPContent(object content)
        {
            string jsonString = JsonConvert.SerializeObject(content);
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
