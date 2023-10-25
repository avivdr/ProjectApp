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
        public async Task<User> Login(string username, string password)
        {
            return new User() { Username = username, Password = password };
        }
    }
}
