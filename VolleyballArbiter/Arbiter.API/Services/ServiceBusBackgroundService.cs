using Arbiter.API.Services.Interfaces;
using Azure.Messaging.ServiceBus;

namespace Arbiter.API.Services
{
    public class ServiceBusBackgroundService : BackgroundService
    {
        private readonly ITextAnalyzerService _textAnalyzerService;
        private readonly IConfiguration _configuration;

        public ServiceBusBackgroundService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _textAnalyzerService = scope.ServiceProvider.GetRequiredService<ITextAnalyzerService>();
            }
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = new ServiceBusClient(_configuration["ServiceBusConnectionString"]);
            var processor = client.CreateProcessor("arbitercalls");

            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
 
            await processor.StartProcessingAsync();   

            while(!stoppingToken.IsCancellationRequested)
            {
                 await Task.Delay(100);
            }

            await processor.StopProcessingAsync();
        }

        public async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string textFromServiceBus = args.Message.Body.ToString();
           
            var dotIndex = textFromServiceBus.IndexOf(".");
            var matchId = Int32.Parse(textFromServiceBus.Substring(0, dotIndex));
            var sentence = textFromServiceBus.Substring(dotIndex + 2);
            try
            {
                await _textAnalyzerService.AnalyzeSentence(matchId, sentence);
            }
            finally
            {
                await args.CompleteMessageAsync(args.Message);
            }  
        }

        public Task ErrorHandler(ProcessErrorEventArgs args)
        {
            throw new Exception("BackgroundService exception");
        }
    }
}
