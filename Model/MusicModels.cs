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
        public string Type { get; set; }
        public string Item { get; set; }
        public string Search { get; set; }
        public string Offset { get; set; }
    }
    public class Status
    {
        public string Version { get; set; }
        public string Success { get; set; }
        public string Source { get; set; }
        public int Rows { get; set; }
        public float ProcessingTime { get; set; }
        public string Api { get; set; }
    }

    #region Objects
    public class Work
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Searchterms { get; set; }
        public string Popular { get; set; }
        public string Recommended { get; set; }
        public string Id { get; set; }
        public string Genre { get; set; }
        public string SearchMode { get; set; }
        public string Catalogue { get; set; }
        public string Catalogue_Number { get; set; }
        public Composer Composer { get; set; }

    }
    public class Composer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Complete_Name { get; set; }
        public string Birth { get; set; }
        public string Death { get; set; }
        public string Epoch { get; set; }
        public string Portrait { get; set; }
    }
    #endregion

    #region Requests
    public class ComposerResult
    {
        public Status Status { get; set; }
        public Request Request { get; set; }
        public List<Composer> Composers { get; set; }
    }

    public class GenreResult
    {
        public Status Status { get; set; }
        public Composer Composer { get; set; }
        public List<string> Genres { get; set; }
    }

    public class WorkResult
    {
        public Status Status { get; set; }
        public Request Request { get; set; }
        public Composer Composer { get; set; }
        public List<Work> Works { get; set; }
    }

    public class OmniSearchResult
    {
        public Status Status { get; set; }
        public Request Request { get; set; }
        public List<OmniSearchItem> Results { get; set; }
        public int Next { get; set; }
    }

    public class OmniSearchItem
    {
        public Composer Composer { get; set; }
        public Work Work { get; set; }
    }
    #endregion    
}
