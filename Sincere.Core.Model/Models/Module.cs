using System;
using System.Collections.Generic;

namespace Sincere.Core.Model.Models
{
    public partial class Module
    {
        public Module()
        {
            InverseParent = new HashSet<Module>();
            ModulePermission = new HashSet<ModulePermission>();
        }

        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public int OrderSort { get; set; }
        public string Description { get; set; }
        public bool IsMenu { get; set; }
        public bool Enabled { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual Module Parent { get; set; }
        public virtual ICollection<Module> InverseParent { get; set; }
        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
    }
}
