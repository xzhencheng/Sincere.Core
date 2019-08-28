using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class OperateLog
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Ipaddress { get; set; }
        public string Description { get; set; }
        public DateTime? LogTime { get; set; }
        public string LoginName { get; set; }
        public int UserId { get; set; }
        public int? UserUId { get; set; }

        public virtual SysUserInfo UserU { get; set; }
    }
}
