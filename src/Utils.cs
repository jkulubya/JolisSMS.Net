using System;
using System.Linq;
using Flurl;
using JolisSMS.Net.Models;

namespace JolisSMS.Net
{
    internal static class Utils
    {
        internal static BroadcastResponse ResponseToBroadcastResponse(string response)
        {
            var broadcastResponse = new BroadcastResponse();
            if (response.Contains("SUCCESS"))
            {
                var indexOfEndBracket = response.IndexOf("]", StringComparison.InvariantCulture);
                var indexOfPipe = response.IndexOf("|", StringComparison.InvariantCulture);
                var lengthOfId = indexOfPipe - indexOfEndBracket - 2;

                broadcastResponse.Scheduled = true;
                broadcastResponse.Response = new Response
                {
                    ScheduleId = Convert.ToInt32(response.Substring(indexOfEndBracket + 2, lengthOfId)),
                    Text = response.Substring(indexOfPipe + 1)
                };
            }
            else
            {
                var messageIndex = response.IndexOf("]", StringComparison.InvariantCulture) + 2;
                broadcastResponse.Scheduled = false;
                broadcastResponse.Error = response.Substring(messageIndex);
            }

            return broadcastResponse;
        }

        internal static string BuildBroadcastUrl(Message message, string baseUrl, string userName, string password)
        {
            var recipients = message.Recipients.Aggregate((acc, next) => acc + "," + next);
            
            var uri = baseUrl.SetQueryParams(new
            {
                action = "broadcast",
                command = "vsms",
                IS_GET = "3",
                message = message.Body,
                password = password,
                to = recipients,
                username = userName,
            });

            return uri;
        }
    }
}