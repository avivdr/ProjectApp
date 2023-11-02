using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<User> Login(string username, string password)
        {
            return _users.FirstOrDefault(x => x.Username == username && x.Password == password);
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
    }
}
