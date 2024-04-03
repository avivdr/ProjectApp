using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProjectApp.Model;

namespace ProjectApp.Services
{
    public class UserService
    {
        private const string Key = "CurrentUser";
        readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        private User user;
        
        public async Task<User> GetUser()
        {
            if (user != null) return user;

            string st = await SecureStorage.Default.GetAsync(Key);
            if (!string.IsNullOrEmpty(st))
                return JsonSerializer.Deserialize<User>(st, jsonOptions);

            return null;
        }
        public async Task SetUser(User value)
        {
            user = value;
            await SecureStorage.Default.SetAsync(Key, JsonSerializer.Serialize(user, jsonOptions));
        }
        
    }
}
