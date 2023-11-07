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
        private List<User> _users = new()
        {
            new() { Username = "vulu123", Password = "12345", Email = "vulu@gmail.com" },
            new() { Username = "bubu123", Password = "baba123", Email = "bubu@gmail.com" }
        };

        public Service()
        {
            httpClient = new HttpClient();
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
            User user = new User() { Password = password, Username = username };
            try
            {
                var stringContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{URL}/Login", stringContent);


                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());

                    case HttpStatusCode.Forbidden:
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
    }
}
