using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.HeadersExchange.Consumer
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
                    exchange: "header-exchange-example",
                    type: ExchangeType.Headers
                    );

            Console.Write("Lütfen header value'sunu giriniz : ");
            string value = Console.ReadLine();

            string queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(
                queue: queueName,
                exchange: "header-exchange-example",
                routingKey: string.Empty,
                new Dictionary<string, object>//Headerlar burada dolar. Publishere göre eşleşmesini istediğine göre doldur
                {
                    //["x-match"]="all",//Default olarak any geçerlidir
                    ["no"] = value
                });

            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
                );

            consumer.Received += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);
            };
        }
    }
}