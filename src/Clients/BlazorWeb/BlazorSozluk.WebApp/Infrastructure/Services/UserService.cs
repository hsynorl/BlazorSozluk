using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure.Results;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.WebApp.Common;
using BlazorSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorSozluk.WebApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient client;

        public UserService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserDetailViewModel> GetUserDetail(Guid? id)
        {
            var userDetail = await client.GetFromJsonAsync<UserDetailViewModel>($"{HostConf.host}/api/user/{id}");
            return userDetail;
        }

        public async Task<UserDetailViewModel> GetUserDetail(string userName)
        {
            var userDetail = await client.GetFromJsonAsync<UserDetailViewModel>($"{HostConf.host} /api/user/username/{userName}");
            return userDetail;
        }

        public async Task<bool> ChangeUserPassword(string oldPassword, string newPassword)
        {
            var command = new ChangePasswordCommand(null, oldPassword, newPassword);
            var httpResponse = await client.PostAsJsonAsync($"{HostConf.host}/api/User/ChnagePassword", command);
            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr);
                    responseStr = validation.FlattenErrors;
                    throw new DataMisalignedException(responseStr);
                }
                return false;
            }
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUser(UserDetailViewModel userDetail)
        {
            var res = await client.PostAsJsonAsync($"{HostConf.host}/api/user/update", userDetail);
            return res.IsSuccessStatusCode;

        }


    }
}
