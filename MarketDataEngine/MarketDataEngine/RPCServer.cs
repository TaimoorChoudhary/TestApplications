using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MarketDataEngine
{
    public class RPCServer
    {
        private static BasicDeliverEventArgs _ea;
        private static IModel _channelRecieve;
        private static IModel _channelTransmit;
        private static IBasicProperties _props;
        private static IBasicProperties _replyProps;
        private static SyntheticDataCreator _dataCreator;

        public static void Main()
        {
            ConnectionFactory factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                };

            IConnection connection = factory.CreateConnection();
            // Reciever
            _channelRecieve = connection.CreateModel();
            // Transmiter
            _channelTransmit = connection.CreateModel();

            // .ExchangeDeclare(exchange, type, durable, auto-delete, IDictionary)
            // Reciever
            _channelRecieve.ExchangeDeclare("md-exchange", ExchangeType.Direct, true, false, null);
            // Transmiter
            _channelTransmit.ExchangeDeclare("md-exchange", ExchangeType.Direct, true, false, null);

            // .QueueDeclare(queue, durable, exclusive, auto-delete, IDictionary)
            // Reciever
            _channelRecieve.QueueDeclare("subreq_queue", false, false, false, null);
            // Transmiter
            _channelTransmit.QueueDeclare("md_queue", false, false, false, null);

            // .QueueBind(Queue, Exchange, RoutingKey)
            // Reciever
            _channelRecieve.QueueBind("subreq_queue", "md-exchange", "DataRequest");
            // Transmiter
            _channelTransmit.QueueBind("md_queue", "md-exchange", "MarketData");

            // Reciever
            _channelRecieve.BasicQos(0, 1, false);

            QueueingBasicConsumer consumer = new QueueingBasicConsumer(_channelRecieve);
            _channelRecieve.BasicConsume("subreq_queue", false, consumer);

            Console.WriteLine(" [x] Awaiting Market Data Subscription requests");

            while (true)
            {
                _ea = (BasicDeliverEventArgs) consumer.Queue.Dequeue();

                byte[] body = _ea.Body;
                _props = _ea.BasicProperties;
                _replyProps = _channelTransmit.CreateBasicProperties();
                _replyProps.CorrelationId = _props.CorrelationId;

                try
                {
                    string message = System.Text.Encoding.UTF8.GetString(body);
                    Console.WriteLine("Subscription request recieved for: ({0})", message);
                    ProcessIncomingMessage(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e);
                }
                finally
                {
                    _channelRecieve.BasicAck(_ea.DeliveryTag, false);
                }
            }

        }

        private static void ProcessIncomingMessage(string message)
        {
            string[] arguments = message.Split('|');

            if (arguments[0].Equals("Subscribe"))
            {
                _dataCreator = new SyntheticDataCreator(arguments[1], 2);
                _dataCreator.TickArrived += new Action<Tick>(OnTickArrived);
                _dataCreator.Subscribe();
            }
            else if (arguments[0].Equals("Unsubscribe"))
            {
                _dataCreator.Unubscribe();
            }
        }

        private static void OnTickArrived(Tick tick)
        {
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(tick.ToString());
            _channelTransmit.BasicPublish("md-exchange", _props.ReplyTo, _replyProps,
                                 responseBytes);
            //_channelTransmit.BasicAck(_ea.DeliveryTag, false);
        }
    }
}
