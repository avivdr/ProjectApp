using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public class WorksUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int WorkId { get; set; }

        public User User { get; set; }
        public Work Work { get; set; }

        public WorksUser()
        {
            Id = 0;
            UserId = 0;
            WorkId = 0;

            if (User != null)
                UserId = User.Id;

            if (Work != null)
                WorkId = Work.Id;
        }
    }
}
