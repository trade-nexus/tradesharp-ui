using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Saves Strategy Statistics calculated using executions
    /// </summary>
    public class OptimizationStatistics
    {
        /// <summary>
        /// Strategy ID for which the stats are calculated
        /// </summary>
        private string _strategyId;

        /// <summary>
        /// Strategy Profit/Loss
        /// </summary>
        private decimal _pnl;

        /// <summary>
        /// Total number of shares bought 
        /// </summary>
        private int _sharesBought;

        /// <summary>
        /// Total number of shares sold
        /// </summary>
        private int _sharesSold;

        /// <summary>
        /// Average Buy Price 
        /// </summary>
        private decimal _avgBuyPrice;

        /// <summary>
        /// Average Sell Price 
        /// </summary>
        private decimal _avgSellPrice;

        /// <summary>
        /// Parameter Description
        /// </summary>
        private string _description;

        #region Properties

        /// <summary>
        /// Strategy ID for which the stats are calculated
        /// </summary>
        public string StrategyId
        {
            get { return _strategyId; }
            set { _strategyId = value; }
        }
        
        /// <summary>
        /// Strategy Profit/Loss
        /// </summary>
        public decimal Pnl
        {
            get { return _pnl; }
            set { _pnl = value; }
        }

        /// <summary>
        /// Total number of shares bought 
        /// </summary>
        public int SharesBought
        {
            get { return _sharesBought; }
            set { _sharesBought = value; }
        }

        /// <summary>
        /// Total number of shares sold
        /// </summary>
        public int SharesSold
        {
            get { return _sharesSold; }
            set { _sharesSold = value; }
        }

        /// <summary>
        /// Average Buy Price 
        /// </summary>
        public decimal AvgBuyPrice
        {
            get { return _avgBuyPrice; }
            set { _avgBuyPrice = value; }
        }

        /// <summary>
        /// Average Sell Price 
        /// </summary>
        public decimal AvgSellPrice
        {
            get { return _avgSellPrice; }
            set { _avgSellPrice = value; }
        }
        
        /// <summary>
        /// Gets Over all strategy position (Long/Short)
        /// </summary>
        public string Position
        {
            get
            {
                int temp = _sharesBought + (-_sharesSold);
                if (temp > 0)
                {
                    return "Long " + temp;
                }
                else if (temp < 0)
                {
                    return "Short " + Math.Abs(temp);
                }
                return "NONE";
            }
        }

        /// <summary>
        /// Parameter Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategyId">Strategy id for which to calculate the statistics</param>
        public OptimizationStatistics(string strategyId)
        {
            _strategyId = strategyId;
            _pnl = default(decimal);
            _sharesBought = default(int);
            _sharesSold = default(int);
            _avgBuyPrice = default(decimal);
            _avgSellPrice = default(decimal);
        }
    }
}
