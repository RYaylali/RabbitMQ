﻿using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.FanoutExchange.Publisher
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(200);
                byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
                channel.BasicPublish(
                    exchange: "fanout-exchange-example",
                    routingKey: string.Empty,
                    body: message);
            }
            Console.Read();
        }
        //isim farketmeksizin bütün kuyruklara mesajı bind eder
    }
}