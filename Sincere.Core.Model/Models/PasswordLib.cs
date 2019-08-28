using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class PasswordLib
    {
        public int Plid { get; set; }
        public bool? IsDeleted { get; set; }
        public string PlUrl { get; set; }
        public string PlPwd { get; set; }
        public string PlAccountName { get; set; }
        public int? PlStatus { get; set; }
        public int? PlErrorCount { get; set; }
        public string PlHintPwd { get; set; }
        public string PlHintquestion { get; set; }
        public DateTime? PlCreateTime { get; set; }
        public DateTime? PlUpdateTime { get; set; }
        public DateTime? PlLastErrTime { get; set; }
        public string Test { get; set; }
    }
}
