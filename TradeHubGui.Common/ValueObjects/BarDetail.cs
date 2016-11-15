using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Contains complete bar detail
    /// </summary>
    public class BarDetail
    {
        private Bar _bar;
        private BarParameters _barParameters;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="bar">Contains bar values</param>
        /// <param name="barParameters">Parameters used to create bar</param>
        public BarDetail(Bar bar, BarParameters barParameters)
        {
            _bar = bar;
            _barParameters = barParameters;
        }

        /// <summary>
        /// Contains Bar values i.e. OPEN,HIGH,LOW,CLOSE
        /// </summary>
        public Bar Bar
        {
            get { return _bar; }
        }

        /// <summary>
        /// Parameters used to create the bar
        /// </summary>
        public BarParameters BarParameters
        {
            get { return _barParameters; }
        }
    }
}
