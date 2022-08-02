using RichardSzalay.MockHttp;
using System;
using Xunit;
using System.Net.Http;
using Domain.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Application.Commands.HomeCommands;
using Domain.Models;

namespace Cwy516TestProject
{
    public class HttpTest : IClassFixture<CustomTestFixture>
    {
        private CustomTestFixture _factory;
        public HttpTest(CustomTestFixture factory)
        {
            this._factory = factory;
        }
        /// <summary>
        /// ����mock
        /// </summary>
        [Fact]
        public async void Test1()
        {
            MockHttpMessageHandler mockHandler = null; //new MockHttpMessageHandler();    //���뱣֤��ַ���Է���
            //mockHandler.When("http://localhost:5100/api/Home/Index").Respond("application/json", JsonConvert.SerializeObject(new { id="1111",name="222" }));//mock����
            //mockHandler.When("http://localhost:5100/api/Home/Index").Throw(new TimeoutException("����"));//����polly
            this._factory.handler = mockHandler;

            var client = this._factory.CreateClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/Home/GetAllUser");//��ǰ��Ŀ��·�ɵ�ַ������ip+port��
            //requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, object>() { { "RequestId", "1234" } }), Encoding.UTF8, "application/json");

            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(new GetAllUserRequestCommand() { RequestId="6666" }), Encoding.UTF8, "application/json");

            var res = await client.SendAsync(requestMessage);
            var respond = JsonConvert.DeserializeObject<GetAllUserResponseCommand>(res.Content.ReadAsStringAsync().Result);
            Assert.True(respond.IsSuccess);
        }

        [Fact]
        public async void IdentityServerTest()
        {
            //var data = new
            //{
            //    client_id = "client",
            //    client_secret = "secret",
            //    grant_type = "password",
            //    username = "cwy",
            //    password = "123456"
            //};
            var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token");
            //����ʹ��FormUrlEncodedContent�ķ�ʽ���ݲ���
            List<KeyValuePair<string, string>> nameVals = new List<KeyValuePair<string, string>>();
            nameVals.Add(new KeyValuePair<string, string>("client_id", "client"));
            nameVals.Add(new KeyValuePair<string, string>("client_secret", "secret"));

            //password��ʽ
            nameVals.Add(new KeyValuePair<string, string>("grant_type", "password"));
            nameVals.Add(new KeyValuePair<string, string>("username", "admin"));
            nameVals.Add(new KeyValuePair<string, string>("password", "123456"));

            //client_credentials��ʽ���û�
            //nameVals.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

            request.Content = new FormUrlEncodedContent(nameVals);
            //request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/x-www-form-urlencoded");
            //request.Content.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var client = this._factory.CreateClient();
            var res = await client.SendAsync(request);
            var respond = JsonConvert.DeserializeObject<IdentityServerTokenResult>(res.Content.ReadAsStringAsync().Result);
        }
    }
}
