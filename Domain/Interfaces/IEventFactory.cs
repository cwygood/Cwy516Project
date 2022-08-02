using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    /// <summary>
    /// 事件操作接口
    /// </summary>
    public interface IEventFactory
    {
        /// <summary>
        /// 增加事件
        /// </summary>
        /// <returns></returns>
        bool AddEvents(List<EventLog> @events);
        /// <summary>
        /// 获取未发布的事件
        /// </summary>
        /// <returns></returns>
        List<EventLog> GetUnPublishedEvents();
        /// <summary>
        /// 推送事件
        /// </summary>
        /// <returns></returns>
        bool PublishEvent(EventLog @event);
        /// <summary>
        /// 消费事件
        /// </summary>
        /// <returns></returns>
        bool ProduceEvent(EventLog @event);
    }
}
