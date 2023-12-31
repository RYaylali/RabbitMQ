﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.FanoutExchange.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: "fanout-exchange-example", 
                type: ExchangeType.Fanout);

            Console.Write("Kuyruk adını giriniz : ");
            string _queueName = Console.ReadLine();
            channel.QueueDeclare(
                queue:_queueName,
                exclusive:false
                );
            channel.QueueBind(
                queue:_queueName,
                exchange: "fanout-exchange-example",
                routingKey:string.Empty
                );
            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue:_queueName,
                autoAck:true,
                consumer:consumer
                );
            consumer.Received += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);
            };
            Console.Read();
        }
    }
}