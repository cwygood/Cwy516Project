using Application.Commands;
using Application.Commands.HomeCommands;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwy516Project.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ITest _test;
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;
        private readonly IMemoryCache _memoryCache;
        public HomeController(ITest test, ILogger<HomeController> logger, IMediator mediator, IRedisCache cache, IMemoryCache memoryCache)
        {
            this._test = test;
            this._logger = logger;
            this._mediator = mediator;
            this._cache = cache;
            this._memoryCache = memoryCache;
        }

        #region Test
        [HttpGet]
        [Route("api/Home/Index")]
        public ActionResult<string> Index()
        {
            System.Threading.Thread.Sleep(1000 * 60);
            this._test.Show();
            this._logger.LogInformation("11111");
            this._cache.GetDatabase().StringSet("TestKey", "Hello");
            this._memoryCache.Set<string>("TestKey", "11111");
            return "helloworld";
        }
        [HttpPost]
        [Route("api/Home/Post")]
        public JsonResult Post()
        {
            var val = this._memoryCache.Get<string>("TestKey");
            return new JsonResult(new
            {
                MyActionResult = val
            });
        }
        #endregion

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Home/GetAllUser")]
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
        [Route("api/Home/GetUserById")]
        //[Authorize(Roles = "admin")]
        public async Task<GetUserResponseCommand> GetUserById(GetUserRequestCommand command)
        {
            return await this._mediator.Send(command);
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Home/GetToken")]
        public async Task<GetTokenResponseCommand> GetToken()
        {
            //传入用户名和密码获取token=>用户名得到角色
            var command = new GetTokenRequestCommand();
            //command.Role = "admin";//登录按照用户的角色赋值
            return await this._mediator.Send(command);
        }
    }
}
