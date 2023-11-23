using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectApp.Model;

namespace ProjectApp.Services
{
    public class Service
    {
        private HttpClient httpClient;
        const string URL = "https://dz7hpq26-7058.euw.devtunnels.ms/OpusOne";
        private JsonSerializerOptions options;
        private List<User> _users = new()
        {
            new() { Username = "vulu123", Pwsd = "12345", Email = "vulu@gmail.com" },
            new() { Username = "bubu123", Pwsd = "baba123", Email = "bubu@gmail.com" }
        };

        public Service()
        {
            httpClient = new HttpClient();
            options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }   

        public async Task<string> GetHello()
        {
            try
            {
                var response = await httpClient.GetAsync($"{URL}/Hello");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

            }
            return "error";
        }

        public async Task<User> Login(string username, string password)
        {
            User user = new User() { Pwsd = password, Username = username, Email = "" };
            var stringContent = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($"{URL}/Login", stringContent);


                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), options);

                    case HttpStatusCode.Unauthorized:
                        return null;

                    default:
                        throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<HttpStatusCode> Register(User user)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($"{URL}/Register", stringContent);

                return response.StatusCode;
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
    }
}
