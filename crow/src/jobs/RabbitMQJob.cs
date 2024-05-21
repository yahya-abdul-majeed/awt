using System.Text;
using System.Text.Json;
using crow.models;
using crow.services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace crow.src.jobs;

public class RabbitMQJob
{
    private readonly ILogger<RabbitMQJob> _logger;
    private readonly ISubmissionService _ss;
    public RabbitMQJob(ILogger<RabbitMQJob> logger, ISubmissionService ss)
    {
        _logger = logger;
        _ss =ss;
    }

    private const string QUEUE_NAME = "rpc_queue";
    private const string EXCHANGE_NAME = "rpc_exchange";
    public void OpenRPCConnection(){
        var hostname_ip = Environment.GetEnvironmentVariable("ASPNETCORE_RABBITMQ_HOSTNAME_IP");
        Console.WriteLine("this is ip: "+hostname_ip);
        //----------//
        var factory = new ConnectionFactory
        {
            DispatchConsumersAsync = true,
            HostName = hostname_ip//added later
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments:null
        );

        // channel.BasicQos(
        //     prefetchSize: 0,
        //     prefetchCount: 1,
        //     global: false
        // );
        var consumer = new AsyncEventingBasicConsumer(channel);
        Console.WriteLine(" [x] Awaiting RPC requests");

        consumer.Received += async (model, ea) => {
            _logger.LogInformation("Message received");

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId; 
            TestResult response = new(); 

            try
            {
                var message = Encoding.UTF8.GetString(body);
                var submission_id = int.Parse(message);
                IsolateJob ij = new(_ss);
                response = await ij.Perform(submission_id);
            } 
            catch(Exception){}
            finally
            {
                var executionResultJSON = JsonSerializer.Serialize(response);
                var responseBytes = Encoding.UTF8.GetBytes(executionResultJSON);
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: props.ReplyTo,
                    basicProperties: replyProps,
                    body: responseBytes
                );
                // channel.BasicAck(deliveryTag: ea.DeliveryTag,multiple:false);
                // BasicAck caused problems
            }

        };

        channel.BasicConsume(
            queue: QUEUE_NAME,
            autoAck: true,
            consumer: consumer
        );

    }






}