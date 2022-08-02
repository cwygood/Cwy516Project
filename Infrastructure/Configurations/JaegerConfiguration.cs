using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class JaegerConfiguration
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 端点
        /// </summary>
        public string Endpoint { get; set; }
        /// <summary>
        /// 最大传输大小
        /// </summary>
        public int MaxPacketSize { get; set; }
        /// <summary>
        /// 是否启用Form数据转成Span
        /// </summary>
        public bool IsFormSpan { get; set; } = false;
        /// <summary>
        /// Form数据最大长度
        /// </summary>
        public int FormValueMaxLength { get; set; } = 100;
        /// <summary>
        /// 是否启用Query数据转成Span
        /// </summary>
        public bool IsQuerySpan { get; set; } = false;
        /// <summary>
        /// Query最大长度
        /// </summary>
        public int QueryValueMaxLength { get; set; } = 100;
        /// <summary>
        /// 是否启用Body数据转span
        /// </summary>
        public bool IsBodySpan { get; set; }
        /// <summary>
        /// body最大长度
        /// </summary>
        public int BodyValueMaxLength { get; set; } = 100;
        /// <summary>
        /// Form或Query中不作为Span的key集合
        /// </summary>
        public string[] NoSpanKeys { get; set; } = new string[] { "password", "pwd" };
    }
}
