using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataEngine
{
    public class Tick 
    {
        #region Private Fields

        private string _symbol;
        private DateTime _dateTime;
        private decimal _bid;
        private int _bidSize;
        private decimal _ask;
        private int _askSize;
        private decimal _trade;
        private int _tradeSize;
        private int _depth;
        private bool _isTrade;
        private bool _isBid;
        private bool _isAsk;
        private bool _isQuote;
        private bool _isFullQuote;
        private string _exchange;

        #endregion Private Fields

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Tick()
        {
        }

        /// <summary>
        /// Accepts quote's name, prices and sizes
        /// </summary>
        public Tick(string symbol, DateTime dateTime, decimal bid, decimal ask, decimal trade, int bidSize, int askSize, int tradeSize)
        {
            this._symbol = symbol;
            this._dateTime = dateTime;
            this._bid = bid;
            this._ask = ask;
            this._trade = trade;
            this._bidSize = bidSize;
            this._askSize = askSize;
            this._tradeSize = tradeSize;
        }

        /// <summary>
        /// Primary values excluding DateTime
        /// </summary>
        public Tick(string symbol, decimal bid, decimal ask, decimal trade, int bidSize, int askSize, int tradeSize)
        {
            this._symbol = symbol;
            this._bid = bid;
            this._ask = ask;
            this._trade = trade;
            this._bidSize = bidSize;
            this._askSize = askSize;
            this._tradeSize = tradeSize;
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string response = "Tick " + " | " + "Symbol: " + _symbol + " | DateTime: " + _dateTime.ToLongTimeString() + " | Bid: " + _bid + 
                " | Ask: " + _ask + " | Bid Size: " + _bidSize + " | Ask Size: " + _askSize;
            return response;
        }

        #region Properties

        /// <summary>
        /// Gets/sets Symbol
        /// </summary>
        public string Symbol
        {
            get
            {
                return this._symbol;
            }
            set
            {
                this._symbol = value;
            }
        }

        /// <summary>
        /// Gets/sets DateTime
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return  this._dateTime;
            }
            set
            {
                this._dateTime = value;
            }
        }

        /// <summary>
        /// Gets/sets Bid
        /// </summary>
        public decimal Bid
        {
            get
            {
                return this._bid;
            }
            set
            {
                this._bid = value;
            }
        }

        /// <summary>
        /// Gets/sets Ask
        /// </summary>
        public decimal Ask
        {
            get
            {
                return this._ask;
            }
            set
            {
                this._ask = value;
            }
        }

        /// <summary>
        /// Gets/sets Trade
        /// </summary>
        public decimal Trade
        {
            get
            {
                return this._trade;
            }
            set
            {
                this._trade = value;
            }
        }

        /// <summary>
        /// Gets/sets BidSize
        /// </summary>
        public int BidSize
        {
            get
            {
                return this._bidSize;
            }
            set
            {
                this._bidSize = value;
            }
        }

        /// <summary>
        /// Gets/sets AskSize
        /// </summary>
        public int AskSize
        {
            get
            {
                return this._askSize;
            }
            set
            {
                this._askSize = value;
            }
        }

        /// <summary>
        /// Gets/sets TradeSize
        /// </summary>
        public int TradeSize
        {
            get
            {
                return this._tradeSize;
            }
            set
            {
                this._tradeSize = value;
            }
        }

        /// <summary>
        /// Gets/sets Depth
        /// </summary>
        public int Depth
        {
            get
            {
                return this._depth;
            }
            set
            {
                this._depth = value;
            }
        }

        /// <summary>
        /// Gets/sets IsBid
        /// </summary>
        public bool IsBid
        {
            get
            {
                return this._isBid;
            }
            set
            {
                this._isBid = value;
            }
        }

        /// <summary>
        /// Gets/sets IsAsk
        /// </summary>
        public bool IsAsk
        {
            get
            {
                return this._isAsk;
            }
            set
            {
                this._isAsk = value;
            }
        }

        /// <summary>
        /// Gets/sets IsTrade
        /// </summary>
        public bool IsTrade
        {
            get
            {
                return this._isTrade;
            }
            set
            {
                this._isTrade = value;
            }
        }

        /// <summary>
        /// Gets/sets IsFullQuote
        /// </summary>
        public bool IsFullQuote
        {
            get
            {
                return this._isFullQuote;
            }
            set
            {
                this._isFullQuote = value;
            }
        }

        /// <summary>
        /// Gets/sets IsQuote
        /// </summary>
        public bool IsQuote
        {
            get
            {
                return this._isQuote;
            }
            set
            {
                this._isQuote = value;
            }
        }

        /// <summary>
        /// Gets/sets Exchange
        /// </summary>
        public string Exchange
        {
            get
            {
                return this._exchange;
            }
            set
            {
                this._exchange = value;
            }
        }

        #endregion Properties
    }
}