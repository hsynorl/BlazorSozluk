using Blazored.LocalStorage;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Infrastructure.Results;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using BlazorSozluk.WebApp.Infrastructure.Extensions;
using BlazorSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorSozluk.WebApp.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient client;
        private readonly ISyncLocalStorageService syncLocalStorage;

        public IdentityService(HttpClient client, ISyncLocalStorageService syncLocalStorage)
        {
            this.client = client;
            this.syncLocalStorage = syncLocalStorage;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

        public string GetUserToken()
        {
            return syncLocalStorage.GetToken();
        }
        public string GetUserName()
        {

            return syncLocalStorage.GetToken();
        }
        public Guid GetUserId()
        {
            return syncLocalStorage.GetUserId();
        }


        public async Task<bool> Login(LoginUserCommand loginUserCommand)
        {
            string responseStr;
            var httpResponse = await client.PostAsJsonAsync("/api/User/login", loginUserCommand);

            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr);
                    responseStr = validation.FlattenErrors;
                    throw new DatabaseValidationExcepiton(responseStr);

                }
                return false;
            }
            responseStr = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<LoginUserViewModel>(responseStr);
            if (!string.IsNullOrEmpty(response.Token))
            {
                syncLocalStorage.SetToken(response.Token);
                syncLocalStorage.SetUsername(response.UserName);
                syncLocalStorage.SetUserId(response.Id);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                   "bearer", response.UserName);

                return true;
            }
            return false;

        }
        public void Logout()
        {
            syncLocalStorage.RemoveItem(LocalStorageExtension.TokenName);
            syncLocalStorage.RemoveItem(LocalStorageExtension.UserName);
            syncLocalStorage.RemoveItem(LocalStorageExtension.UserId);

            client.DefaultRequestHeaders.Authorization = null;

        }
    }
}
