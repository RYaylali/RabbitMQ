using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.MessageTepmlate.Publisher
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            #region P2P (Point to Point) Tasarımı
            //string queueName = "example-p2p-queue";

            //channel.QueueDeclare(
            //    queue:queueName,
            //    durable:false,
            //    exclusive:false,
            //    autoDelete:false
            //    );
            //byte[] message = Encoding.UTF8.GetBytes("Merhaba");
            //channel.BasicPublish(
            //    exchange:string.Empty,
            //    routingKey:queueName,
            //    body:message);

            #endregion
            #region Publish/Subscribe (Pub/Sub) Tasarımı
            //string exchangeName = "example-pubsub-exchange";
            //channel.ExchangeDeclare(
            //    exchange: exchangeName,
            //    type: ExchangeType.Fanout);
            //for (int i = 0; i < 100; i++)
            //{
            //    await Task.Delay(300);
            //    byte[] message = Encoding.UTF8.GetBytes("Merhaba");
            //    channel.BasicPublish(
            //        exchange: exchangeName,
            //        routingKey: string.Empty,
            //        body: message);
            //}
            #endregion
            #region Work Queue (İş Kuyruğu) Tasarımı
            //string queueName = "example-work-queue";
            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);
            //for (int i = 0; i < 100; i++)
            //{
            //    await Task.Delay(300);
            //    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
            //    channel.BasicPublish(
            //        exchange: string.Empty,
            //        routingKey: queueName,
            //        body: message);
            //}

            #endregion
            #region Response/Request Tasarımı
            string queueName = "example-request-response-queue";

            channel.QueueDeclare(
                queue: queueName,
                exclusive: false,
                autoDelete: false,
                durable: false
                );
            string replyName = channel.QueueDeclare().QueueName;//Response alabilmek çin kuyruğun ismini tanımladık.
            string correlationId = Guid.NewGuid().ToString();//Response olarak cevap aldığımız response mesaj ile bizim request attığımız mesajın aynı olduğunu kontrol etmek için 
            #region Request Mesajını oluşturma ve gönderme
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.CorrelationId = correlationId;
            basicProperties.ReplyTo = replyName;//responsenin döneceği kuyruk
            for (int i = 0; i < 100; i++)
            {
                byte[] message = Encoding.UTF8.GetBytes("Merhaba :" + i);
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: queueName,
                    body: message,
                    basicProperties: basicProperties);
            }
            #endregion
            #region Response Kuyruğu Dinleme
            EventingBasicConsumer consumer = new(channel);//Burada request atan poblisher a consumer tarafından dönen response mesajının işlenime işlemi yapılır
            channel.BasicConsume(
                queue: replyName,
                autoAck: true,
                consumer: consumer);
            consumer.Received += (sender, e) =>
            {
                if (e.BasicProperties.CorrelationId==correlationId)//BUrada bizim request mesajındaki id ile responsedeki mesajın aynı olup olmadığı kontrol edilim mesaj işleniyor
                {
                    Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
                }
            };
            #endregion
            #endregion
            Console.Read();
        }
    }
}