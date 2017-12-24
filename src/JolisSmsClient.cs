using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JolisSMS.Net.Models;
using Flurl;

namespace JolisSMS.Net
{
    public class JolisSmsClient: IDisposable
    {
        public JolisSmsClient(string userName, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password);
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(userName);
            }
            
            this.HttpClient = new HttpClient();
            this.DisposeHttpClient = true;
            this.Password = password;
            this.UserName = userName;
        }
        
        public JolisSmsClient(string userName, string password, HttpClient httpClient)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password);
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(userName);
            }

            this.HttpClient = httpClient;
            this.Password = password;
            this.UserName = userName;
        }

        private bool DisposeHttpClient { get; set; } = false;
        private HttpClient HttpClient { get; set; }
        private string JolisUri { get; set; } = "https://secure.jolis.net/api.php";
        private string Password { get; set; }
        private string UserName { get; set; }

        public async Task<BroadcastResponse> Broadcast(Message message)
        {
            var uri = Utils.BuildBroadcastUrl(message, JolisUri, this.UserName, this.Password);

            var httpResponse = await HttpClient.GetStringAsync(uri.ToString());

            var broadcastResponse = Utils.ResponseToBroadcastResponse(httpResponse);

            return broadcastResponse;
        }

        public void Dispose()
        {
            if (DisposeHttpClient)
            {
                HttpClient?.Dispose();
            }
        }
    }
}
