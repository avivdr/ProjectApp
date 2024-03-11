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
        readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
        };

        private User user;
        public User User
        {
            get
            {
                User u = null;
                Task.Run(async () =>
                {
                    string st = await SecureStorage.Default.GetAsync(Key);
                    if (!string.IsNullOrEmpty(st))
                        u = JsonSerializer.Deserialize<User>(st, options);
                });

                return u ?? user;
            }
            set
            {
                user = value;
                Task.Run(async () =>
                {
                    await SecureStorage.Default.SetAsync(Key, JsonSerializer.Serialize(user, options));
                });
            }
        }
    }
}
