using System;
using System.Threading.Tasks;
using JolisSMS.Net;
using JolisSMS.Net.Models;

namespace test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var m = new Message();
            m.SetBody("hello james").AddRecipient("256777105269");
            var smsClient = new JolisSmsClient("altx", "R47v9vTjhrJ7Pd");

            var result = await smsClient.Broadcast(m);
            Console.WriteLine($"{result.Response.Text} - {result.Response.ScheduleId}");
        }
    }
}