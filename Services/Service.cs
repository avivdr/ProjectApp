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
        private List<User> _users = new()
        {
            new() { Username = "vulu123", Password = "12345", Email = "vulu@gmail.com" },
            new() { Username = "bubu123", Password = "baba123", Email = "bubu@gmail.com" }
        };

        public async Task<User> Login(string username, string password)
        {
            return _users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }
    }
}
