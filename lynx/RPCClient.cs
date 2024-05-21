using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using lynx.Models;

namespace lynx
{
    public class RPCClient
    {
        private const string QUEUE_NAME = "rpc_queue";
        private const string EXCHANGE_NAME = "rpc_exchange";

        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<TestResult>> callbackMapper = new();

        public RPCClient()
        {
            var hostname_ip = Environment.GetEnvironmentVariable("ASPNETCORE_RABBITMQ_HOSTNAME_IP");
            var factory = new ConnectionFactory();
            factory.HostName = hostname_ip;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            replyQueueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                    return;
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var response = JsonSerializer.Deserialize<TestResult>(message);
                tcs.SetResult(response);
            };

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true
                );
        }
        public Task<TestResult> CallAsync(int submissionId, CancellationToken cancellationToken = default)
        {
            IBasicProperties props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            //var messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(submissionId));
            var messageBytes = Encoding.UTF8.GetBytes(submissionId.ToString());
            var tcs = new TaskCompletionSource<TestResult>();

            callbackMapper.TryAdd(correlationId, tcs);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: QUEUE_NAME,
                basicProperties: props,
                body: messageBytes
                );

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }
    }
}
