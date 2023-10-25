using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectApp.Services;

namespace ProjectApp.Model
{
    public class Request
    {
        public string type { get; set; }
        public string item { get; set; }
        public string search { get; set; }
        public string offset { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }
    public class Status
    {
        public string version { get; set; }
        public string success { get; set; }
        public string source { get; set; }
        public int rows { get; set; }
        public float processingtime { get; set; }
        public string api { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }

    #region Objects
    public class Work
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string searchterms { get; set; }
        public string popular { get; set; }
        public string recommended { get; set; }
        public string id { get; set; }
        public string genre { get; set; }
        public string searchmode { get; set; }
        public string catalogue { get; set; }
        public string catalogue_number { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }

    }
    public class Composer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string complete_name { get; set; }
        public string birth { get; set; }
        public string death { get; set; }
        public string epoch { get; set; }
        public string portrait { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }
    #endregion

    #region Requests
    public class ComposerResult
    {
        public Status status { get; set; }
        public Request request { get; set; }
        public List<Composer> composers { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }

    public class GenreResult
    {
        public Status status { get; set; }
        public Composer composer { get; set; }
        public List<string> genres { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }

    public class WorkResult
    {
        public Status status { get; set; }
        public Request request { get; set; }
        public Composer composer { get; set; }
        public List<Work> works { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }

    public class OmniSearchResult
    {
        public Status status { get; set; }
        public Request request { get; set; }
        public List<OmniSearchItem> results { get; set; }
        public int next { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }

    public class OmniSearchItem
    {
        public Composer composer { get; set; }
        public Work work { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, OpenOpusService.options);
        }
    }
    #endregion    
}
