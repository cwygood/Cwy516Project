using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Common.SnowFlake;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EventBus
{
    public class EventFacotry : IEventFactory
    {
        private readonly IDbHelper _context;
        private readonly IOptionsMonitor<EventConfiguration> _options;
        public EventFacotry(IDbHelper context, IOptionsMonitor<EventConfiguration> options)
        {
            this._context = context;
            this._options = options;
        }
        public bool AddEvents(List<EventLog> @events)
        {
            if (@events.Count == 0)
            {
                return false;
            }
            var createSql = @$"create table if not exists {_options.CurrentValue.EventBusTableName}
                            (
                            Id bigint auto_increment primary key comment '自增主键',
                            EventId bigint not null default 0 comment '事件id',
                            EventStatus TINYINT not null default 0 comment '事件状态',
                            SendCount int not null default 0 comment '发送次数',
                            Content text comment '事件内容',
                            CreateTime datetime comment '创建时间'
                            )";
            this._context.Excute(createSql, null);
            foreach (var eventLog in @events)
            {
                if (eventLog.EventId <= 0)
                {
                    eventLog.EventId = SnowFlake.CreateSnowFlakeId();
                }
                string sql = @$"insert into {_options.CurrentValue.EventBusTableName}(eventId,eventStatus,sendCount,content,createTime)
                            values({eventLog.EventId},{(int)eventLog.EventStatus},{eventLog.SendCount},'{eventLog.Content}','{eventLog.CreateTime}')";
                this._context.Excute(sql, null);//todo:此处考虑事务
            }
            return true;
        }

        public List<EventLog> GetUnPublishedEvents()
        {
            throw new NotImplementedException();
        }

        public bool ProduceEvent(EventLog @event)
        {
            throw new NotImplementedException();
        }

        public bool PublishEvent(EventLog @event)
        {
            throw new NotImplementedException();
        }
    }
}
