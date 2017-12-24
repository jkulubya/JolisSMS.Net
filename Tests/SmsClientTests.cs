using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JolisSMS.Net;
using JolisSMS.Net.Models;
using RichardSzalay.MockHttp;
using Xunit;

namespace Tests
{
    public class SmsClientTests
    {
        [Fact]
        public async Task HitCorrectUrlWithSingleRecipient()
        {
            var mockHttp = new MockHttpMessageHandler();
            var request = mockHttp.When(HttpMethod.Get, "https://secure.jolis.net/api.php")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"command", "vsms"},
                    {"IS_GET", "3"},
                    {"username", "testuser"},
                    {"password", "testpassword"},
                    {"action", "broadcast"},
                    {"message", "ping"},
                    {"to", "256772000001"}

                })
                .Respond("text/html", "[SUCCESS] 998899|Broadcast Scheduled");

            var smsClient = new JolisSmsClient("testuser", "testpassword", mockHttp.ToHttpClient());
            var m = new Message();
            m.SetBody("ping").AddRecipient("256772000001");
            await smsClient.Broadcast(m);
            
            Assert.Equal(1, mockHttp.GetMatchCount(request));
        }
        
        [Fact]
        public async Task HitCorrectUrlWithMultipleRecipients()
        {
            var mockHttp = new MockHttpMessageHandler();
            var request = mockHttp.When(HttpMethod.Get, "https://secure.jolis.net/api.php")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"command", "vsms"},
                    {"IS_GET", "3"},
                    {"username", "testuser"},
                    {"password", "testpassword"},
                    {"action", "broadcast"},
                    {"message", "ping"},
                    {"to", "256772000001,256772000002"}

                })
                .Respond("text/html", "[SUCCESS] 998899|Broadcast Scheduled");

            var smsClient = new JolisSmsClient("testuser", "testpassword", mockHttp.ToHttpClient());
            var m = new Message();
            m.SetBody("ping").AddRecipients(new List<string>
            {
                "256772000001",
                "256772000002"
            });
            await smsClient.Broadcast(m);
            
            Assert.Equal(1, mockHttp.GetMatchCount(request));
        }

        [Fact]
        public async Task ParseSuccessResponseSuccessfully()
        {
            var mockHttp = new MockHttpMessageHandler();
            var request = mockHttp.When(HttpMethod.Get, "https://secure.jolis.net/api.php")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"command", "vsms"},
                    {"IS_GET", "3"},
                    {"username", "testuser"},
                    {"password", "testpassword"},
                    {"action", "broadcast"},
                    {"message", "ping"},
                    {"to", "256772000001"}

                })
                .Respond("text/html", "[SUCCESS] 998899|Broadcast Scheduled");

            var smsClient = new JolisSmsClient("testuser", "testpassword", mockHttp.ToHttpClient());
            var m = new Message();
            m.SetBody("ping").AddRecipient("256772000001");
            var result = await smsClient.Broadcast(m);
            
            Assert.Equal(result.Scheduled, true);
            Assert.Equal(result.Response.ScheduleId, 998899);
            Assert.Equal(result.Response.Text, "Broadcast Scheduled");
        }
        
        [Fact]
        public async Task ParseErrorResponseSuccessfully()
        {
            var mockHttp = new MockHttpMessageHandler();
            var request = mockHttp.When(HttpMethod.Get, "https://secure.jolis.net/api.php")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"command", "vsms"},
                    {"IS_GET", "3"},
                    {"username", "testuser"},
                    {"password", "testpassword"},
                    {"action", "broadcast"},
                    {"message", "ping"},
                    {"to", "256772000001"}

                })
                .Respond("text/html", "[ERROR] Insufficient credits remaining");

            var smsClient = new JolisSmsClient("testuser", "testpassword", mockHttp.ToHttpClient());
            var m = new Message();
            m.SetBody("ping").AddRecipient("256772000001");
            var result = await smsClient.Broadcast(m);
            
            Assert.Equal(result.Scheduled, false);
            Assert.Equal(result.Error, "Insufficient credits remaining");
        }
    }
}