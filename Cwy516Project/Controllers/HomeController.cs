using Application.Commands;
using Application.Commands.HomeCommands;
using Domain.Interfaces;
using Elasticsearch.Net;
using Infrastructure.Common.Consts;
using Infrastructure.Common.Mq;
using Infrastructure.Common.Tools;
using Infrastructure.Configurations;
using Infrastructure.ElasticSearch;
using Infrastructure.Jaeger;
using Infrastructure.Kafka;
using Infrastructure.Quartz;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cwy516Project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ITest _test;
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly IMemoryCache _memoryCache;
        private readonly TaskPoolManager _taskPoolManage;

        private readonly IOptionsMonitor<ConsulConfiguration> _options;
        private readonly IESServer _esServer;
        private readonly IKafkaHelper _kafkaHelper;
        private readonly IEventFactory _eventFactory;
        private readonly RabbitMqClient _rabbitMqClient;

#if Linux
        public HomeController(ITest test, ILogger<HomeController> logger, IMediator mediator, IRedisCache cache, IMemoryCache memoryCache, IOptionsMonitor<ConsulConfiguration> options, IESServer eSServer,
            IKafkaHelper kafkaHelper, RabbitMqClient rabbitMqClient, TaskPoolManager taskPoolManager, IEventFactory eventFactory)
        {
            this._test = test;
            this._logger = logger;
            this._mediator = mediator;
            this._cache = cache;
            this._memoryCache = memoryCache;
            this._options = options;
            this._esServer = eSServer;
            this._kafkaHelper = kafkaHelper;
            this._rabbitMqClient = rabbitMqClient;
            this._taskPoolManage = taskPoolManager;
            this._eventFactory = eventFactory;
        }
#else
        public HomeController(ITest test, ILogger<HomeController> logger, IMediator mediator, IRedisCache cache, IMemoryCache memoryCache, TaskPoolManager taskPoolManager)
        {
            this._test = test;
            this._logger = logger;
            this._mediator = mediator;
            this._cache = cache;
            this._memoryCache = memoryCache;
            this._taskPoolManage = taskPoolManager;
        }
#endif
        #region Test
        [HttpGet]
        public ActionResult<string> Index()
        {
            Console.WriteLine($"Service:{this._options.CurrentValue.ServiceName},Address:{this._options.CurrentValue.ServiceHost}:{_options.CurrentValue.ServicePort}");
            //System.Threading.Thread.Sleep(1000 * 60);

            this._test.Show();
            this._logger.LogInformation("11111");
            this._cache.SetString("TestKey", "Hello");
            //this._cache.GetDatabase().StringSet("TestKey", "Hello");
            this._memoryCache.Set<string>("TestKey", "11111");
            return "helloworld";
        }
        /// <summary>
        /// 服务调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Get()
        {
            var tracer = new MyTracer("/MyTracer");
            //tracer.SetOperationName("GetMyOperation");
            tracer.SetTag("First", "11111111111111");
            tracer.SetTag("Second", "22222222222222222");
            tracer.Dispose();//必须销毁才能上报span，否则不会记录到jaeger中
            return "AAA";
        }
        /// <summary>
        /// 事件总线
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task EventBus()
        {
            await _taskPoolManage.SchedulerJob("ttt", "fff", "0/5 * * * * ?");
            await _taskPoolManage.SchedulerJob("111", "kkk", "0/6 * * * * ?");
            await _taskPoolManage.SchedulerJob("EventBus", "ssss", "0/7 * * * * ?");
        }
        [HttpPost]
        public string PPP()
        {
            this._eventFactory.AddEvents(new List<Domain.Models.EventLog>()
            {
                new Domain.Models.EventLog()
                {
                    EventStatus=Domain.Enums.EventStatus.UnPublished,
                    Content="Test",
                    CreateTime=DateTime.Now,
                    SendCount=10
                }
            });
            return "AAA";
        }
        /// <summary>
        /// Consul服务调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetConsul()
        {
            return new JsonResult(new
            {
                name = this._options.CurrentValue.ServiceName,
                port = this._options.CurrentValue.ServicePort
            });
        }
        /// <summary>
        /// consul验证请求转发
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponseCommand> ConsulRequest(ConsulRequestCommand command)
        {
            return await this._mediator.Send(command);
        }
        /// <summary>
        /// consul健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> HealthCheck()
        {
            return "health";
        }
        [HttpGet]
        public ActionResult<string> OcelotA()
        {
            return "ServiceA" + Guid.NewGuid().ToString("N");//后台ocelot启用了缓存，所以在缓存时间的范围内，返回的内容不变
        }
        [HttpGet]
        public ActionResult<string> OcelotB()
        {
            return "ServiceB" + Guid.NewGuid().ToString("N");
        }
        [HttpPost]
        public JsonResult Post()
        {
            var ss = this._cache.GetString("TestKey");
            var val = this._memoryCache.Get<string>("TestKey");
            return new JsonResult(new
            {
                MyActionResult = val
            });
        }
        [HttpPost]
        public string UploadFile(string name, IFormFile file)
        {
            return "上传成功！";
        }

#region ElasticSearch
        [HttpGet]
        public async Task<IEnumerable<ESDocument>> ESLinqSearch()
        {
            var list = await this._esServer.ElasticLinqClient.SearchAsync<ESDocument>(
                f => f.Index("cwy516project").Query(
                    op => op.MatchAll() 
                    //op.Match(
                    //    ss => ss.Field(
                    //        qq => qq.Name.StartsWith("Test")))
                            ));
            return list.Documents;
        }
        [HttpGet]
        public async Task<JObject> ESJsonSearch()
        {
            var jsonObject = new { query = new { match = new { Name = "Test" } } };
            string json = JsonConvert.SerializeObject(jsonObject);
            var res = await this._esServer.ElasticJsonClient.SearchAsync<StringResponse>("Test", PostData.String(json));
            return JObject.Parse(res.Body);
        }
        [HttpPost]
        public async Task<string> ESCreateLinqIndex(string indexName, int numberOfShards = 5, int numberOfReplicas = 1)
        {
            //管道
            var res = await this._esServer.ElasticLinqClient.Indices.CreateAsync(indexName, f => f.InitializeUsing(new IndexState()
            {
                Settings = new IndexSettings()
                {
                    NumberOfShards = numberOfShards,
                    NumberOfReplicas = numberOfReplicas
                }
            })
            .Map<Test>(p => p.AutoMap()));

            var sss = await this._esServer.ElasticLinqClient.IndexDocumentAsync<ESDocument>(new ESDocument() { data = "1111",Id="my516project" });
            return sss.Index;
        }
        [HttpPost]
        public async Task<string> ESCreateJsonIndex(string indexName, string id)
        {
            var ss = new
            {
                mappings = new
                {
                    properties = new
                    {
                        name = new
                        {
                            type="text"
                        }
                    }
                }
            };
            var sss = await this._esServer.ElasticJsonClient.IndexAsync<StringResponse>(indexName, id, PostData.Serializable(new Test() { Id = "123", Name = "ttt" }));
            var res = await this._esServer.ElasticJsonClient.Indices.CreateAsync<StringResponse>(indexName, PostData.Serializable(ss));

            return sss.Body;
        }
#endregion

#region Kafka

        [HttpPost]
        public async Task<string> PublishKafka(string topic, Test test)
        {
            return await this._kafkaHelper.PublishAsync<Test>(topic, test);
        }

        [HttpPost]
        public async Task<Test> SubscribeKafka(string[] topics)
        {
            return await this._kafkaHelper.SubscribeAsync<Test>(topics);
        }

#endregion

#region RabbitMq集群

        [HttpPost]
        public void SendRabbitMq(string message)
        {
            this._rabbitMqClient.PushMessage("rabbitmqTest", message, "20210818");
        }
        [HttpPost]
        public void ReceiveRabbitMq()
        {
            this._rabbitMqClient.GetMessage("rabbitmqTest", "20210818");
        }

#endregion

#endregion

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<GetAllUserResponseCommand> GetAllUser(GetAllUserRequestCommand command)
        {
            return await this._mediator.Send(command);
        }
        /// <summary>
        /// 查询单个用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<GetUserResponseCommand> GetUserById(GetUserRequestCommand command)
        {
            return await this._mediator.Send(command);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AddUserResponseCommand> AddUser(AddUserRequestCommand command)
        {
            var ss = ModelState.IsValid;
            return await this._mediator.Send(command);
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<GetTokenResponseCommand> GetToken()
        {
            //传入用户名和密码获取token=>用户名得到角色
            var command = new GetTokenRequestCommand();
            //command.Role = "admin";//登录按照用户的角色赋值
            return await this._mediator.Send(command);
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileContentResult> CreateValidateCode()
        {
            var key = base.HttpContext.Request.Query["validateKey"];
            FileContentResult contentResult = null;
            using (var stream = ValidateCodeHelper.Create(out string code))
            {
                var buffer = stream.ToArray();
                contentResult = File(buffer, "image/jpeg");
                this._cache.SetString($"ValidateKey_{key}", code, TimeSpan.FromMinutes(3), Consts.MainRedisKey);
            }
            return await Task.FromResult(contentResult);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginResponseCommand> Login(LoginRequestCommand command)
        {
            return await this._mediator.Send(command);
        }
    }
}
