﻿using System;
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
        const string URL = "https://dz7hpq26-7058.euw.devtunnels.ms/OpusOne";
        readonly JsonSerializerOptions options;
        const string CURRENT_USER_KEY = "CurrentUser";

        public Service()
        {
            httpClient = new HttpClient();
            options = new JsonSerializerOptions()
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
            catch (Exception)
            {

            }
            return "error";
        }

        public async Task<User> GetCurrentUser()
        {
            string st = await SecureStorage.GetAsync(CURRENT_USER_KEY);
            if (string.IsNullOrEmpty(st))
                return null;
            return JsonSerializer.Deserialize<User>(st, options);
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
                    return JsonSerializer.Deserialize<List<Composer>>(content, options);
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
                    return JsonSerializer.Deserialize<OmniSearchDTO>(content, options);
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
                    return JsonSerializer.Deserialize<OmniSearchDTO>(content, options);
                }
            }
            catch (Exception) { }

            return null;
        }

        public async Task<HttpStatusCode> UploadPost(Post post, FileResult file = null)
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
                    multipartFormContent.Add(content, "file", file.FileName);
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

                        await SecureStorage.Default.SetAsync("CurrentUser", st);
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
            string userJson = JsonSerializer.Serialize(user, options);
            var stringContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($@"{URL}/Register", stringContent);
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    await SecureStorage.SetAsync(CURRENT_USER_KEY, content);
                return response.StatusCode;
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
    }
}
