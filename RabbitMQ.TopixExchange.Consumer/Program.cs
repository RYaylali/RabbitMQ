using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.TopixExchange.Consumer
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
                exchange: "topic-exchange-example",
                type: ExchangeType.Topic
                );
            Console.WriteLine("Dinlenecek  topic formatını belirleyiniz: ");
            string topic = Console.ReadLine();
            string queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(
                queue: queueName,
                exchange: "topic-exchange-example",
                routingKey:topic
                );
            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue:queueName,
                autoAck:true,
                consumer
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