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
        readonly HttpClient httpClient;
        private const string ServerURL = $"{URL}/OpusOne";
        public const string URL = "https://vxfmp0h3-7058.uks1.devtunnels.ms";

        readonly JsonSerializerOptions jsonOptions;
        const string CURRENT_USER_KEY = "CurrentUser";

        public Service()
        {
            httpClient = new HttpClient();
            jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
            };
        }   

        public async Task<string> GetHello()
        {
            try
            {
                var response = await httpClient.GetAsync($"{ServerURL}/Hello");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

            }
            return "error";
        }

        public async Task<User> GetCurrentUser()
        {
            string st = await SecureStorage.Default.GetAsync(CURRENT_USER_KEY);
            if (string.IsNullOrEmpty(st))
                return null;
            return JsonSerializer.Deserialize<User>(st, jsonOptions);
        }

        public async Task<List<Composer>> SearchComposersByName(string query)
        {
            if (query.Length < 4) return null;

            try
            {
                var response = await httpClient.GetAsync($@"{ServerURL}/SearchComposerByName/{query}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Composer>>(content, jsonOptions);
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<OmniSearchDTO> OmniSearch(string query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 3)
                return null;

            try
            {
                var response = await httpClient.GetAsync($@"{ServerURL}/OmniSearch/{query}/0");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<OmniSearchDTO>(content, jsonOptions);
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<OmniSearchDTO> NextOmniSearch()
        {
            try
            {
                var response = await httpClient.GetAsync($@"{ServerURL}/NextOmniSearch");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<OmniSearchDTO>(content, jsonOptions);
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<Post> GetPostById(int id)
        { 
            Post post;
            try
            {
                var postResponse = await httpClient.GetAsync($@"{ServerURL}/GetPostById/{id}");
                if (postResponse.StatusCode == HttpStatusCode.OK)
                {
                    string content = await postResponse.Content.ReadAsStringAsync();
                    post = JsonSerializer.Deserialize<Post>(content, jsonOptions);
                }
                else return null;

                return post;
            }
            catch (Exception) { }

            return null;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            try
            {
                var response = await httpClient.GetAsync(@$"{ServerURL}/GetAllPosts");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Post>>(content, jsonOptions);
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<HttpStatusCode> UploadComment(Comment Comment)
        {
            try
            {
                StringContent stringContent = new(JsonSerializer.Serialize(Comment, jsonOptions));
                var response = await httpClient.PostAsync($"{URL}/UploadComment", stringContent);
                return response.StatusCode;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> UploadPost(Post post, FileResult file = null)
        {
            try
            {
                var multipartFormContent = new MultipartFormDataContent();

                if (file != null)
                {
                    byte[] bytes;
                    using (MemoryStream ms = new())
                    {
                        var stream = await file.OpenReadAsync();
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    var content = new ByteArrayContent(bytes);
                    multipartFormContent.Add(content, "file", file.FileName);
                }                

                var stringContent = new StringContent(JsonSerializer.Serialize(post, jsonOptions), Encoding.UTF8, "application/json");
                multipartFormContent.Add(stringContent, "post");

                var response = await httpClient.PostAsync($"{ServerURL}/UploadPost", multipartFormContent);
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
            var stringContent = new StringContent(JsonSerializer.Serialize(user, jsonOptions), Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($"{ServerURL}/Login", stringContent);


                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string st = await response.Content.ReadAsStringAsync();

                        await SecureStorage.Default.SetAsync("CurrentUser", st);
                        return JsonSerializer.Deserialize<User>(st, jsonOptions);                        

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
            string userJson = JsonSerializer.Serialize(user, jsonOptions);
            var stringContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($@"{ServerURL}/Register", stringContent);
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    await SecureStorage.Default.SetAsync(CURRENT_USER_KEY, content);
                return response.StatusCode;
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
    }
}
