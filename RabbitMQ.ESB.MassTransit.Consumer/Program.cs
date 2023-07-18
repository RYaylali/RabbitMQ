using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer.Consumer;

namespace RabbitMQ.ESB.MassTransit.Consumer
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
                factory.ReceiveEndpoint(queueName, endpoint =>
                {
                    endpoint.Consumer<ExampleConsumer>();
                });
            });
            await bus.StartAsync();
            Console.Read();
        }
    }
}