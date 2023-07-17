using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.HeadersExchange.Publisher
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
                exchange: "header-exchange-example",
                type: ExchangeType.Headers);

            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(200);
                byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
                Console.Write("Lütfen header value'sunu giriniz : ");
                string value = Console.ReadLine();

                IBasicProperties basicProperties = channel.CreateBasicProperties();//Headersları doldurmak için 
                basicProperties.Headers = new Dictionary<string, object>
                {
                    ["no"] = value
                };

                channel.BasicPublish(
                    exchange: "header-exchange-example",
                    routingKey: string.Empty,
                    body: message,
                    basicProperties: basicProperties
                    );
                Console.Read();
                //headers a göre eşleşen headerları eşler ve o kuyruğa gönderir
            }
        }
    }
}