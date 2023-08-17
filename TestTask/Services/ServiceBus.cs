using Microsoft.Azure.ServiceBus;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;
        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMessageAsync(int userId)
        {
            string messageBody = "";
            IQueueClient client = new QueueClient(_configuration["AzureServiceBusConnectionString"], _configuration["QueueName"]);
           
            try
            {
                var json = JsonSerializer.Serialize(userId, new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                messageBody = JsonSerializer.Serialize(userId, new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });

            }
            catch (Exception e)
            {
                var p = e.Message;
            }

            var message = new Message(Encoding.UTF8.GetBytes(messageBody))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };
            await client.SendAsync(message);
        }
    }
}
