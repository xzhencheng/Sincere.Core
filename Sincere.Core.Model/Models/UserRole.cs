using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual Role Role { get; set; }
        public virtual SysUserInfo User { get; set; }
    }
}
