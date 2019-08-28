using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class Advertisement
    {
        public int Id { get; set; }
        public DateTime Createdate { get; set; }
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Remark { get; set; }
    }
}
