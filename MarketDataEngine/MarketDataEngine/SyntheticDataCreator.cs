using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace MarketDataEngine
{
    /// <summary>
    /// Creates dummy data to be used only for testing purpose
    /// </summary>
    public class SyntheticDataCreator
    {
        // Time interval to be used between data messages.
        private double _interval;

        // Maximun spread between generated prices.
        private decimal _spread;

        // Maximun spread between generated bid/ask sizes.
        private int _sizeSpread;

        // Symbol for which the data is generated.
        private string _symbol;

        // It controls the data frequency
        private Timer _sendDataTimer;

        // Fired after creating a successful tick
        public event Action<Tick> TickArrived;

        private decimal _bid = 1.2576m;
        private decimal _ask = 1.2596m;
        private int _bidSize = 1000;
        private int _askSize = 1000;
        private int _count = 0;
        private int _stopageCount = 2;

        /// <summary>
        /// Gets/Sets Symbol value
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        /// <summary>
        /// Default Constructo
        /// </summary>
        public SyntheticDataCreator()
        {
            _interval = 1000;
            _spread = 0.0002m;
            _sizeSpread = (int) (_spread*1000000);
            _symbol = "Symbol";
            _stopageCount = 2;
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="stopageCount"></param>
        public SyntheticDataCreator(string symbol, int stopageCount)
        {
            _interval = 1000;
            _spread = 0.0002m;
            _sizeSpread = (int)(_spread * 1000000);
            _symbol = symbol;
            _stopageCount = stopageCount;
            _stopageCount = 2;
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="spread"></param>
        /// <param name="symbol"></param>
        /// <param name="stopageCount"></param>
        public SyntheticDataCreator(double interval, decimal spread, string symbol, int stopageCount)
        {
            _interval = interval;
            _spread = spread;
            _sizeSpread = (int)(_spread * 1000000);
            _symbol = symbol;
            _stopageCount = stopageCount;
        }

        /// <summary>
        /// Starts sending the synthetic data to the subscriber
        /// </summary>
        public bool Subscribe()
        {
            try
            {
                _sendDataTimer = new Timer();
                _sendDataTimer.Interval = _interval;
                _sendDataTimer.Elapsed += GenerateData;
                _sendDataTimer.Start();
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occured while trying to subscribe for synthetic data.", exception);
                return false;
            }
        }

        /// <summary>
        /// Stops sending the synthetic data to the subscriber
        /// </summary>
        public bool Unubscribe()
        {
            try
            {
                _sendDataTimer = new Timer();
                _sendDataTimer.Elapsed -= GenerateData;
                _sendDataTimer.Stop();
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occured while trying to subscribe for synthetic data.", exception);
                return false;
            }
        }

        /// <summary>
        /// Generates the Tick data required by the subscriber
        /// </summary>
        private void GenerateData(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                if (_count > 0)
                {
                    _sendDataTimer.Stop();
                    Console.WriteLine("Timer Stopped");
                }
                _count++;
                GenerateBid();
                GenerateAsk();

                Tick tick = new Tick(_symbol, DateTime.UtcNow, _bid, _ask, 0.0m, _bidSize, _askSize, 0);
                if (TickArrived != null)
                {
                    TickArrived(tick);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occured while trying to generate synthetic data.", exception);
            }
        }

        /// <summary>
        /// Generates random bid price
        /// </summary>
        private void GenerateBid()
        {
            try
            {
                Random random = new Random();

                if (!random.Next(0, 2).Equals(0))
                {
                    _bid += _spread;
                    _bidSize += _sizeSpread;
                }
                else
                {
                    _bid -= _spread;
                    _bidSize -= _sizeSpread;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occured while trying to create synthetic bid.", exception);
            }
        }

        /// <summary>
        /// Generates random ask price
        /// </summary>
        private void GenerateAsk()
        {
            try
            {
                Random random = new Random();

                if (!random.Next(0, 2).Equals(0))
                {
                    _ask += _spread;
                    _askSize += _sizeSpread;
                }
                else
                {
                    _ask -= _spread;
                    _askSize -= _sizeSpread;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occured while trying to create synthetic ask.", exception);
            }
        }
    }
}
