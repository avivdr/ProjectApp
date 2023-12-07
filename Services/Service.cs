using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProjectApp.Model;

namespace ProjectApp.Services
{
    public class Service
    {
        private HttpClient httpClient;
        const string URL = "https://dz7hpq26-7058.euw.devtunnels.ms/OpusOne";
        readonly JsonSerializerOptions options;

        public Service()
        {
            httpClient = new HttpClient();
            options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
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

        public async Task<HttpStatusCode> Post(Post post, FileResult file = null)
        {
            try
            {
                var multipartFormContent = new MultipartFormDataContent();

                if (file != null)
                {
                    byte[] bytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var stream = await file.OpenReadAsync();
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    var content = new ByteArrayContent(bytes);
                    multipartFormContent.Add(content, "file", "fileName");
                }                

                var stringContent = new StringContent(JsonSerializer.Serialize(post, options), Encoding.UTF8, "application/json");
                multipartFormContent.Add(stringContent, "post");

                var response = await httpClient.PostAsync($"{URL}/UploadPost", multipartFormContent);
                return response.StatusCode;

            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<User> Login(string username, string password)
        { 
            User user = new() { Password = password, Username = username };
            var stringContent = new StringContent(JsonSerializer.Serialize(user, options), Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($"{URL}/Login", stringContent);


                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                            string st = await response.Content.ReadAsStringAsync();
                            return JsonSerializer.Deserialize<User>(st, options);                        

                    case HttpStatusCode.Unauthorized:
                        return null;

                    default:
                        throw new Exception(response.StatusCode.ToString());
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
                var response = await httpClient.PostAsync($@"{URL}/Register", stringContent);

                return response.StatusCode;
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
    }
}
