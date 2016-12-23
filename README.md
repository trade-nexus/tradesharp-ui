# TradeSharp - UI #

This repository contains all the front-end code for TradeSharp. TradeSharp is a C# based data feed and broker neutral Algorithmic Trading Platform that lets trading firms or individuals automate any rules based trading strategies in stocks, forex and ETFs.

### Table of contents ###

  * [Installation](#installation)
  * [Reporting Bugs](#bugs)
    
***

### Installation ###

To install TradeSharp, follow the steps given [here](https://github.com/trade-nexus/tradesharp-core#installation)

### Usage ###

Once the TradeSharp has started, the dashboard page (see Figure 1 below) is loaded. Menu on the right allows the user to connect with multiple market data and order execution providers. On the levft of the dashborad page, there are tabs for navigating between Dashboard, Strategy Runner, Orders and Data Downloader views. 

![TradeSharp Dashboard](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/dashboard.png)
Figure 1: TradeSharp Dashboard

To get live data from the market data provider for any symbol, create a new market scanner (if not already created) for that provider using the **New Market Scanner** on the dashboard view and add the symbols to the scanner. See Figure 2 below.

![TradeSharp Dashboard with Multiple Market Scanners](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/dashboard-with-scanner-windows.png)
Figure 2: TradeSharp Dashboard with Multiple Market Scanners

### Bugs

Please report bugs [here](https://github.com/trade-nexus/bugs)
