using crow.src.jobs;

namespace crow
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private RabbitMQJob _rabbitmq;
        public Worker(ILogger<Worker> logger, RabbitMQJob rabbitmq)
        {
            _logger = logger;
            _rabbitmq = rabbitmq;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitmq.OpenRPCConnection();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}