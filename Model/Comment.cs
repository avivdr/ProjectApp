using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public int CreatorId { get; set; }

        public string Content { get; set; } = "";

        public DateTime UploadDateTime { get; set; } = DateTime.Now;

        public User Creator { get; set; }

        public Post Post { get; set; }

        [JsonIgnore] public string DateTimeString
        {
            get
            {
                if (UploadDateTime.Date == DateTime.Now.Date)
                    return UploadDateTime.ToString("HH:mm");

                if (UploadDateTime.Year == DateTime.Now.Year)
                    return UploadDateTime.ToString("dd/MM");

                return UploadDateTime.ToString("dd/MM/yyyy");
            }
        }
    }
}
