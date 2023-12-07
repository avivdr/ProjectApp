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
    public enum Period
    {
        Medieval,
        Renaissance,
        Baroque,
        Classical,
        Early_Romantic,
        Romantic,
        Late_Romantic,
        _20th_Century,
        Post_War,
        _21st_Century,
    }

    public enum Genre
    {
        Keyboard,
        Chamber,
        Orchestral,
        Stage,
        Vocal
    }

    public class OpenOpusService
    {
        readonly HttpClient client;
        const string URL = @"https://api.openopus.org";
        public readonly JsonSerializerOptions options;

        public OpenOpusService()
        {
            client = new HttpClient();
            options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }

        public List<string> Periods = new List<string>()
        {
            "Medieval",
            "Renaissance",
            "Baroque",
            "Classical",
            "Early Romantic",
            "Romantic",
            "Late Romantic",
            "20th Century",
            "Post-War",
            "21st Century"
        };

        public List<string> Genres = new List<string>()
        {
            "Keyboard",
            "Chamber",
            "Orchestral",
            "Stage",
            "Vocal"
        };
        public static List<Work> FilterThing(List<Work> list)
        {
            return list.Where(work => work.Genre != "Keyboard" &&
            (work.Title.ToLower().Contains("piano") || work.Title.ToLower().Contains("keyboard"))).ToList();
        }

        public static List<Work> FilterKeyboard(List<Work> list)
        {
            return list.Where(work => work.Genre == "Keyboard").ToList();
        }

        //useful functions

        //for search
        public async Task<OmniSearchResult> OmniSearch(string query)
        {
            try
            {
                var response = await client.GetAsync($"{URL}/omnisearch/{query}/0.json");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<OmniSearchResult>(content);
                    if (result != null && result.Status.Success == "true")
                        return result;
                }
            }
            catch (Exception) { }
            return null;
        }

        public async Task<WorkResult> WorksByComposerID(int composerID)
        {
            try
            {
                var response = await client.GetAsync($"{URL}/work/list/composer/{composerID}/Genre/all.json");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<WorkResult>(content);
                    if (result != null && result.Status.Success == "true")
                        return result;
                }
            }
            catch (Exception) { }
            return null;
        }

        public async Task<ComposerResult> PopularComposers()
        {
            try
            {
                var response = await client.GetAsync($"{URL}/composer/list/pop.json");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ComposerResult>(content);
                }
            }
            catch (Exception) { }
            return null;
        }

        public async Task<ComposerResult> ComposersInPeriod(Period period)
        {
            try
            {
                var response = await client.GetAsync($"{URL}/composer/list/epoch/{Periods[(int)period]}.json");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ComposerResult>(content);
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
