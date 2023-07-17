using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ_Publisher
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            //Rabbitmq sunucuna bağlantı oluşturuldu.
            ConnectionFactory factory = new();
            factory.Uri = new("amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz");//Bu bağlantıyı CloudAMQP adresinde ücretsin clouddan sağladık

            //Bağlantıyı aktifleştirme ve kanal açma 
            using IConnection connection = factory.CreateConnection(); //using işlemi iconnection sinterface IDisposable özelliğine sahip olduğu için using ile işlem yapılmakta
            using IModel channel = connection.CreateModel();

            //Queue oluşturma
            channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);//Durable parametresi: kuyruktaki mesajların kalıcılığını gösterir. Exlusive parametresi: işlemin sadece bu özellik için oluşturulduğunu ve sonrasında silineceği işlemlerde true olarak kullanılır. False olursa birden fazla channel tarafından kullanılabiliir anlamına gelir. AutoDelete parametresi: Kuyruğun içersindeki tüm mesajlar tüketildiğinde kuyruğun silinip silinmeyeceğini belirtir. Durable parametresi: Message durability öz. için açılır rabbitmq sunucusu kapandığında kuyruktaki verilerin silinemesi için
            IBasicProperties properties = channel.CreateBasicProperties();//Message durability ile ilgili
            properties.Persistent = true;//Message durability ile ilgili
            //Queue ye mesaj gönderme---> Rabbitmq kuyruğa mesajları byte türünden kabul etmektedir. Bu yüzden gönderilecek mesajları byte türüne çevir
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(200);
                byte[] message = Encoding.UTF8.GetBytes("Merhaba");
                channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message,basicProperties:properties);//exchange: default olarak bırakılırsa direkt exchange göre işlem yapar. routingkey: Gönderilecek kuyruk. body: kuyruğa gönderilecek mesajı ifade eder. basicproperties parametresi: message durability ile ilgilidir.
            }

            Console.Read();

        }
    }
}