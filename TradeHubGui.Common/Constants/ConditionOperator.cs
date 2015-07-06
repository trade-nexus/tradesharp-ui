using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Constants
{
    public enum ConditionOperator
    {
        [Description("Equal To")]
        Equals,

        [Description("Greater Than")]
        Greater,

        [Description("Lesser Than")]
        Less
    }
}
