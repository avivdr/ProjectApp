using ProjectApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Post
    {
        private readonly List<string> imageTypes = new() { ".jpg", ".jfif", ".png", ".gif" };
        private readonly List<string> videoTypes = new() { ".mp4", ".mov", ".avi", ".wmv" };
        private readonly List<string> audioTypes = new() { ".wav", ".mp3", ".m4a" };

        public int Id { get; set; }

        public int CreatorId { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public DateTime UploadDateTime { get; set; } = DateTime.Now;

        public string FileExtension { get; set; }

        public int? ComposerId { get; set; }

        public int? WorkId { get; set; }

        public List<Comment> Comments { get; set; } = new();

        public User Creator { get; set; }

        public Composer Composer { get; set; }

        public Work Work { get; set; }

        [JsonIgnore] public string File => string.IsNullOrEmpty(FileExtension) ? "" : $"{Service.WwwRoot}/{Id}{FileExtension}";
        [JsonIgnore] public bool IsFile => !string.IsNullOrEmpty(FileExtension);
        [JsonIgnore] public bool IsImage => imageTypes.Contains(FileExtension);
        [JsonIgnore] public bool IsAudio => audioTypes.Contains(FileExtension);
        [JsonIgnore] public bool IsVideo => videoTypes.Contains(FileExtension);
        [JsonIgnore] public string TagString => Work?.TitleWithComposersName ?? Composer?.CompleteName;
        [JsonIgnore] public bool IsComposerImage => Composer != null;

        [JsonIgnore] public string DateTimeString
        {
            get
            {
                if (UploadDateTime.Date == DateTime.Now.Date)
                    return UploadDateTime.ToString("HH:mm");

                if(UploadDateTime.Year == DateTime.Now.Year)
                    return UploadDateTime.ToString("dd/MM");

                return UploadDateTime.ToString("dd/MM/yyyy");
            }
        }
            
    }
}
