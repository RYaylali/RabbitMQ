using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace RabbitMQ.ESB.MassTransit.Publisher
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            string rabbitMQUri = "amqps://enxfcdpz:J3TV52eCntg2BYVk0Oh7dEjvjoIV3evK@toad.rmq.cloudamqp.com/enxfcdpz";
            string queueName = "example-queue";
            IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(rabbitMQUri);
            });
            ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));
            Console.Write("Göndereceğiniz mesajı giriniz : ");
            string message = Console.ReadLine();
            await sendEndpoint.Send<IMessage>(new ExampleMessage()//sendEndpoint ile tek bir kuyruğa mesaj göndermeyi sağladık 
            {
                Text=message
            });
            Console.Read();
        }
    }
}