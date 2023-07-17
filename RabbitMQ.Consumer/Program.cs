using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ_Conssumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Bağlantı oluşturma 
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");

            //Bağlantı aktifleştirme ve kanal açma
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            //Queue oluşturma
            channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);//Buradaki kuyruk yapısı publisher ile aynı özelliklere sahip olmalıdır. durable: message durability le ilgilidir publisher ile aynı olmakı

            //Queue den mesaj okuma 
            EventingBasicConsumer consumer = new(channel);//Bu eventen intance işlemi kuyrukta her hangi bir mesaj olursa tetiklenip okunmasını sağlamak içindir
            channel.BasicConsume("example-queue", autoAck: false, consumer);//AutoAck parameetresi: Kuyruktaki mesaj alındıktan sonra mesajın silinip silinmeyeceğini belirtir
            channel.BasicQos(0, 1, false);//Fair Durability öz.dir. Consumerlere eşit mesaj gider yüklerini eşitler ve böylece sabit perforans ve consumerlerde kısmi açlık önlenir.1.parametre: byte tipinden boyutu 0 sınırsız demek, 2.parametre: tek seferde iletilecek mesaj sayısı 3.parametre: Tüm consumerlar için mi yoksa çağrı yapılan consumer için mi geçerli olduğunu belirler
            consumer.Received += (sender, e) =>
            {
                //Burası kuyrukdan gelen mesajın işlendiği yerdir
                //e.Body : Kuyruktaki mesajın bütünsel olarak getirecek 
                //e.Body.Span veya e.Body.ToArray();Kuyrukdaki mesajı byte türünden getirecek
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
                channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);//multiple parametresi: false olursa sadece bu mesajla ilgili işlemler onaylanması halinde silinir. true olursa bundan önceki tüm onaylı mesajlar silinir. deliveryTag parametresi: Bildirimde bulunulacak mesaj hakkında bildirilen uniq bir değerdir.
                //BasicAck rabitmq mesaj gönderir.
            };
            Console.Read();
        }
    }
}