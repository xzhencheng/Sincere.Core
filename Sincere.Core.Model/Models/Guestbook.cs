using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class Guestbook
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public DateTime Createdate { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Qq { get; set; }
        public string Body { get; set; }
        public string Ip { get; set; }
        public bool Isshow { get; set; }
    }
}
