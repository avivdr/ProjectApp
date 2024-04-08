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
        readonly UserService userService;
        readonly JsonSerializerOptions jsonOptions;

        private const string URL = $"{WwwRoot}/OpusOne";
        public const string WwwRoot = "https://vxfmp0h3-7058.uks1.devtunnels.ms";

        public Service(UserService _userService)
        {
            httpClient = new HttpClient();
            userService = _userService;
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
                var response = await httpClient.GetAsync($"{URL}/Hello");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception) { }

            return "error";
        }

        public async Task<List<Composer>> SearchComposersByName(string query)
        {
            if (query.Length < 4) return null;

            try
            {
                var response = await httpClient.GetAsync($@"{URL}/SearchComposerByName/{query}");
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
                var response = await httpClient.GetAsync($@"{URL}/OmniSearch/{query}/0");
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
                var response = await httpClient.GetAsync($@"{URL}/NextOmniSearch");
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
                var postResponse = await httpClient.GetAsync($@"{URL}/GetPostById/{id}");
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
                var response = await httpClient.GetAsync(@$"{URL}/GetAllPosts");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Post>>(content, jsonOptions);
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<StatusEnum> UploadComment(Comment Comment)
        {
            try
            {
                string json = JsonSerializer.Serialize(Comment, jsonOptions);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{URL}/UploadComment", stringContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return StatusEnum.OK;
                        case HttpStatusCode.Unauthorized: 
                        return StatusEnum.Unauthorized;
                    default:
                        return StatusEnum.Error;
                }
            }
            catch (Exception)
            {
                return StatusEnum.Error;
            }
        }

        public async Task<StatusEnum> UploadPost(Post post, FileResult file = null)
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

                var response = await httpClient.PostAsync($"{URL}/UploadPost", multipartFormContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return StatusEnum.OK;
                    case HttpStatusCode.Unauthorized:
                        return StatusEnum.Unauthorized;
                    default:
                        return StatusEnum.Error;
                }
            }
            catch (Exception)
            {
                return StatusEnum.Error;
            }
        }

        public async Task<User> Login(string username, string password)
        { 
            User user = new() { Password = password, Username = username };
            var stringContent = new StringContent(JsonSerializer.Serialize(user, jsonOptions), Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($"{URL}/Login", stringContent);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string st = await response.Content.ReadAsStringAsync();

                        user = JsonSerializer.Deserialize<User>(st, jsonOptions);
                        await userService.SetUser(user);
                        return user;                        

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

        public async Task<StatusEnum> Register(User user)
        {
            string userJson = JsonSerializer.Serialize(user, jsonOptions);
            var stringContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($@"{URL}/Register", stringContent);
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    await userService.SetUser(JsonSerializer.Deserialize<User>(content, jsonOptions));
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return StatusEnum.OK;
                    case HttpStatusCode.Conflict:
                        return StatusEnum.Conflict;
                    default:
                        return StatusEnum.Error;
                }
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
    }
}
