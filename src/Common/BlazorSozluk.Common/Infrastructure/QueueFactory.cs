using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure
{
    public static class QueueFactory
    {
        public static void SendMessageToExchange(string exchangeName, string exchangeType, string queeueName, object obj)
        {
            var channel = CreateBasicConsumer().EnsureExchange(
                exchangeName,
                exchangeType)
                .EnsureQueue(
                exchangeName,
                queeueName).Model;

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: queeueName,
                basicProperties: null,
                body: body);
        }

        public static EventingBasicConsumer CreateBasicConsumer()
        {
            var factory = new ConnectionFactory() { HostName = SozlukConstants.RabbitMQHost };
            var conncection = factory.CreateConnection();
            var channel = conncection.CreateModel();
            return new EventingBasicConsumer(channel);
        }


        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer,
                                                           string exchangName,
                                                           string exchangeType = SozlukConstants.DefaultExcanhgeType)
        {
            consumer.Model.ExchangeDeclare(exchangName, exchangeType, durable: false, autoDelete: false);
            return consumer;
        }


        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer,
                                                          string exchangName,
                                                          string queueName)
        {
            consumer.Model.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        null);
            consumer.Model.QueueBind(queueName, exchangName, queueName);

            return consumer;
        }
    }
}
