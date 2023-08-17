using Azure.Messaging.ServiceBus;
using System.Text;
using Microsoft.Azure.ServiceBus;
using TestTask.Data;

namespace TestTask.Services
{
    public class OrderQueueListener : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly DataContext context;
        private ServiceBusProcessor processor;
        private IQueueClient _orderQueueClient;

        public OrderQueueListener(IConfiguration configuration, IServiceScopeFactory factory)
        {
            this.configuration = configuration;
            context = factory.CreateScope().ServiceProvider.GetRequiredService<DataContext>(); 
        }

        public async Task Handle(Message message, CancellationToken cancelToken)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var body = Encoding.UTF8.GetString(message.Body);
            int customerId;

            if (int.TryParse(body, out customerId))
            {
                var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
                if (customer != null)
                {
                    customer.OrdersCount++;
                    context.SaveChanges();
                }
            }
            
            await _orderQueueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
        }

        public virtual Task HandleFailureMessage(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            if (exceptionReceivedEventArgs == null)
                throw new ArgumentNullException(nameof(exceptionReceivedEventArgs));

            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messageHandlerOptions = new MessageHandlerOptions(HandleFailureMessage)
            {
                MaxConcurrentCalls = 5,
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(10)
            };
            _orderQueueClient = new QueueClient(configuration["AzureServiceBusConnectionString"], configuration["QueueName"]);
            _orderQueueClient.RegisterMessageHandler(Handle, messageHandlerOptions);

            Console.WriteLine($"{nameof(OrderQueueListener)} service has started.");

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"{nameof(OrderQueueListener)} service has stopped.");

            await _orderQueueClient.CloseAsync().ConfigureAwait(false);
        }
    }
}
