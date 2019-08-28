using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class Topic
    {
        public Topic()
        {
            TopicDetail = new HashSet<TopicDetail>();
        }

        public int Id { get; set; }
        public string TLogo { get; set; }
        public string TName { get; set; }
        public string TDetail { get; set; }
        public string TSectendDetail { get; set; }
        public bool TIsDelete { get; set; }
        public int TRead { get; set; }
        public int TCommend { get; set; }
        public int TGood { get; set; }
        public DateTime TCreatetime { get; set; }
        public DateTime TUpdatetime { get; set; }
        public string TAuthor { get; set; }

        public virtual ICollection<TopicDetail> TopicDetail { get; set; }
    }
}
