using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.MessageTemplates.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            #region P2P (Point to Point) Tasarımı
            //string queueName = "example-p2p-queue";

            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);
            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue:queueName,
            //    autoAck:false,
            //    consumer:consumer);
            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            //};
            #endregion
            #region Publish/Subscribe (Pub/Sub) Tasarımı
            //string exchangeName = "example-pubsub-exchange";
            //channel.ExchangeDeclare(
            //    exchange: exchangeName,
            //    type: ExchangeType.Fanout);
            //string queueName = channel.QueueDeclare().QueueName;
            //channel.QueueBind(
            //    queue:queueName,
            //    exchange:exchangeName,
            //    routingKey:string.Empty);
            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue:queueName,
            //    autoAck:false,
            //    consumer:consumer);
            //channel.BasicQos(//Consumerlere yük dağılımı için eşit miktarda yük dağıtmak için kullanılır.
            //    prefetchCount:1,//gidecek mesaj sayısı
            //    prefetchSize:0,//gidecek mesajın bytes türünden boyutu 0 olunca sınırsız olur
            //    global:false);
            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            //};
            #endregion
            #region Work Queue (İş Kuyruğu) Tasarımı
            //string queueName = "example-work-queue";
            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);
            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue: queueName,
            //    autoAck: true,
            //    consumer: consumer);
            //channel.BasicQos(
            //    prefetchCount: 1,
            //    prefetchSize: 0,
            //    global: false);
            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            //};
            #endregion
            #region Response/Request Tasarımı
            string queueName = "example-request-response-queue";

            channel.QueueDeclare(
                queue: queueName,
                exclusive: false,
                autoDelete: true,
                durable: false
                );
            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
            consumer.Received += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);
                //Burada diğer mesaj işleme işlemleri yapılır 
                //Alt tarafta publisher e gönderilecek mesaj hazırlanır
                 byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem Tamamlandı. {message}");
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.CorrelationId = e.BasicProperties.CorrelationId;
                channel.BasicPublish(
                    exchange:string.Empty,
                    routingKey:e.BasicProperties.ReplyTo,
                    basicProperties:properties,
                    body:responseMessage);
            };
            #endregion
            Console.Read();
        }
    }
}