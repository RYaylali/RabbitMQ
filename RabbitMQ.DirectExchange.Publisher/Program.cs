using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.DirectExchange.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct-exchange", type: ExchangeType.Direct);
            while (true)
            {
                Console.WriteLine("Mesaj : ..");
                string message = Console.ReadLine();
                byte[] byteMessage = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "direct-exchange",
                    routingKey: "direct-queue-exchange",
                    body: byteMessage
                    );
            }

            Console.Read();
        }
        //Direkt kuyruk ismi ile aynı olan kuyruğa mesaj gönderir o yüzden bind işlemine gerek yok çünkü kuyruk belli
    }
}