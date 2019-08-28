using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class TopicDetail
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string TdLogo { get; set; }
        public string TdName { get; set; }
        public string TdContent { get; set; }
        public string TdDetail { get; set; }
        public string TdSectendDetail { get; set; }
        public bool TdIsDelete { get; set; }
        public int TdRead { get; set; }
        public int TdCommend { get; set; }
        public int TdGood { get; set; }
        public DateTime TdCreatetime { get; set; }
        public DateTime TdUpdatetime { get; set; }
        public int TdTop { get; set; }
        public string TdAuthor { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
