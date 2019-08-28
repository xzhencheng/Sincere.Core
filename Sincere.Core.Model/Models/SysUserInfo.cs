using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class SysUserInfo
    {
        public SysUserInfo()
        {
            OperateLog = new HashSet<OperateLog>();
            UserRole = new HashSet<UserRole>();
        }

        public int UId { get; set; }
        public string ULoginName { get; set; }
        public string ULoginPwd { get; set; }
        public string URealName { get; set; }
        public int UStatus { get; set; }
        public string URemark { get; set; }
        public DateTime UCreateTime { get; set; }
        public DateTime UUpdateTime { get; set; }
        public DateTime ULastErrTime { get; set; }
        public int UErrorCount { get; set; }
        public string Name { get; set; }
        public int? Sex { get; set; }
        public int? Age { get; set; }
        public DateTime? Birth { get; set; }
        public string Addr { get; set; }
        public bool? TdIsDelete { get; set; }

        public virtual ICollection<OperateLog> OperateLog { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
