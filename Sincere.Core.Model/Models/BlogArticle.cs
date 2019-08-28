using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class BlogArticle
    {
        public int BId { get; set; }
        public string Bsubmitter { get; set; }
        public string Btitle { get; set; }
        public string Bcategory { get; set; }
        public string Bcontent { get; set; }
        public int Btraffic { get; set; }
        public int BcommentNum { get; set; }
        public DateTime BUpdateTime { get; set; }
        public DateTime BCreateTime { get; set; }
        public string BRemark { get; set; }
    }
}
