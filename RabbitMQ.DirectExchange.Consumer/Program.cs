using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.DirectExchange.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();
            //1.adım
            channel.ExchangeDeclare(exchange: "direct-exchange", type: ExchangeType.Direct);
            //2. adım
            string queueName = channel.QueueDeclare().QueueName;
            //3. adım
            channel.QueueBind(
                queue: queueName,
                exchange: "direct-exchange",
                routingKey: "direct-queue-exchange");
            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue:queueName,
                autoAck:true,
                consumer:consumer);
            consumer.Received += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);
            };

        }
        //1 .adım : Publisher de ki exchange ile birebir aynı isimde ve type a sahip bir exchange tanımlanmalıdır.
        //2. adım : Publisher tarafından routing keyde bulunan değerdeki kuyruğa gönderilen mesajları, kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz gerekmektedir. Bunun için öncelikle kuyruk oluşturulmalıdır.
        //3. adım :
    }
}