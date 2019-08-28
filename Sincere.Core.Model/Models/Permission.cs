using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class Permission
    {
        public Permission()
        {
            ModulePermission = new HashSet<ModulePermission>();
        }

        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderSort { get; set; }
        public int? Mid { get; set; }
        public int? Pid { get; set; }
        public bool? IsButton { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
    }
}
