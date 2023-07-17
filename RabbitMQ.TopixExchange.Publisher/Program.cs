using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.TopixExchange.Publisher
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange:"topic-exchange-example",
                type:ExchangeType.Topic
                );

            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(200);
                byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
                Console.WriteLine("Topic Belirleyiniz :");
                string topic = Console.ReadLine();
                channel.BasicPublish(
                    exchange: "topic-exchange-example",
                    routingKey:topic,
                    body:message
                    );
            }
            Console.Read();
        }
        //Kuyrukların routing keyine içinde yer alan isimlere veri göndermeyii sağlar
    }
}