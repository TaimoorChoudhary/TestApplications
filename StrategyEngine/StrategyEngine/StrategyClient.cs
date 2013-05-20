using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StrategyEngine
{
    internal class StrategyClient
    {
        private IConnection connection;
        private IModel _channelRecieve;
        private IModel _channelTransmit;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public StrategyClient()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            connection = factory.CreateConnection();
            //_channelTransmit = connection.CreateModel();
            //replyQueueName = _channelTransmit.QueueDeclare();
            //consumer = new QueueingBasicConsumer(_channelTransmit);
            //_channelTransmit.BasicConsume(replyQueueName, false, consumer);

            // Reciever
            _channelRecieve = connection.CreateModel();
            // Transmiter
            _channelTransmit = connection.CreateModel();

            //// .ExchangeDeclare(exchange, type, durable, auto-delete, IDictionary)
            //// Reciever
            //_channelRecieve.ExchangeDeclare("md-exchange", ExchangeType.Direct, true, false, null);
            //// Transmiter
            //_channelTransmit.ExchangeDeclare("md-exchange", ExchangeType.Direct, true, false, null);

            // .QueueDeclare(queue, durable, exclusive, auto-delete, IDictionary)
            // Reciever
            _channelRecieve.QueueDeclare("md_queue", false, false, false, null);
            // Transmiter
            _channelTransmit.QueueDeclare("subreq_queue", false, false, false, null);

            // .QueueBind(Queue, Exchange, RoutingKey)
            // Reciever
            _channelRecieve.QueueBind("md_queue", "md-exchange", "MarketData");
            // Transmiter
            _channelTransmit.QueueBind("subreq_queue", "md-exchange", "DataRequest");

            // Reciever
            _channelRecieve.BasicQos(0, 1, false);

            consumer = new QueueingBasicConsumer(_channelRecieve);
            _channelRecieve.BasicConsume("md_queue", false, consumer);
        }

        public string Call(string message)
        {
            string corrId = Guid.NewGuid().ToString();
            IBasicProperties props = _channelTransmit.CreateBasicProperties();
            props.ReplyTo = "MarketData";
            props.CorrelationId = corrId;

            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            _channelTransmit.BasicPublish("md-exchange", "DataRequest", props, messageBytes);

            while (true)
            {
                BasicDeliverEventArgs ea =
                    (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                try
                {
                    //if (ea.BasicProperties.CorrelationId == corrId)
                    {
                        Console.WriteLine(System.Text.Encoding.UTF8.GetString(ea.Body));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e);
                }
                finally
                {
                    _channelRecieve.BasicAck(ea.DeliveryTag, false);
                }
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }

    internal class RPC
    {
        public static void Main()
        {
            StrategyClient rpcClient = new StrategyClient();

            Console.WriteLine(" [x] Requesting Market Data for:(MSFT)");
            string response = rpcClient.Call("Subscribe|MSFT");
            Console.WriteLine(" [.] Got '{0}'", response);

            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();

            rpcClient.Close();
        }
    }
}