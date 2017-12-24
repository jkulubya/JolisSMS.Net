using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JolisSMS.Net.Models
{
    public class Message
    {
        internal string Body { get; set; }
        internal List<string> Recipients { get; set; } = new List<string>();
    }

    public static class MessageExtensions
    {
        public static Message SetBody(this Message message, string body)
        {
            if (body.Length > 160)
            {
                throw new ArgumentException("The body text must not be longer than 160 characters.");
            }
            message.Body = body;

            return message;
        }
        
        public static Message AddRecipient(this Message message, string phoneNumber)
        {
            //TODO test if number is valid
            message.Recipients.Add(phoneNumber);

            return message;
        }

        public static Message AddRecipients(this Message message, IEnumerable<string> phoneNumbers)
        {
            //TODO test if all numbers are valid, if they aren't throw all of them away
            message.Recipients.AddRange(phoneNumbers);

            return message;
        }
    }
}