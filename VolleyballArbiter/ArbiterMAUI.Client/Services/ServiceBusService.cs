using ArbiterMAUI.Client.Services.Interfaces;
using Azure.Messaging.ServiceBus;

namespace ArbiterMAUI.Client.Services
{
    public class ServiceBusService : IServiceBusService
    {
        public void SendMessage(string message)
        {
            var connectionstring = "";
            var client = new ServiceBusClient(connectionstring);
            var sender = client.CreateSender("");

            sender.SendMessageAsync(new ServiceBusMessage(message));
        }
    }
}
