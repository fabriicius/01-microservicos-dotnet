using System.Text;
using AutoMapper;
using ItemService.Dtos;
using ItemService.EventProcessor;
using ItemService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ItemService.RabbitMqClient
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _nomeDaFila;
        private readonly IConnection _connection;
        private IModel _channel;
        private IProcessarEnvento _processarEvento;

        public RabbitMqSubscriber(IConfiguration configuration,
        IProcessarEnvento processarEvento)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = "rabbitmq-service",
                Port = 5672
            }.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeDaFila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _nomeDaFila ,
            exchange: "trigger",
            routingKey: "");
            _processarEvento = processarEvento;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer consumer = new (_channel);
            // consumer.Received += HandleEvent;
            consumer.Received += (ModuleHandle , ea) =>
            {
                ReadOnlyMemory<byte> body = ea.Body;
                string mensagem = Encoding.UTF8.GetString(body.ToArray());
                _processarEvento.Processa(mensagem);
            };
            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        //  private void HandleEvent(object ModuleHandle, BasicDeliverEventArgs ea) {
        //          ReadOnlyMemory<byte> body = ea.Body;
        //         string mensagem = Encoding.UTF8.GetString(body.ToArray());
        //         _processarEvento.Processa(mensagem);
        //  }

    }
}