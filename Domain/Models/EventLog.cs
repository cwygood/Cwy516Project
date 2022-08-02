using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class EventLog
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public long EventId { get; set; }
        /// <summary>
        /// 事件状态
        /// </summary>
        public EventStatus EventStatus { get; set; }
        /// <summary>
        /// 事件发送次数
        /// </summary>
        public int SendCount { get; set; }
        /// <summary>
        /// 事件内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
