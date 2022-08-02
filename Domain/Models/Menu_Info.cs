using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Menu_Info : BaseModel
    {
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父级菜单
        /// </summary>
        public long ParentId { get; set; } 
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
