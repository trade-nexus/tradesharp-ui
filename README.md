# TradeSharp - UI #

This repository contains all the front-end code for TradeSharp. TradeSharp is a C# based data feed and broker neutral Algorithmic Trading Platform that lets trading firms or individuals automate any rules based trading strategies in stocks, forex and ETFs.

### Table of contents ###

  * [Installation](#installation)
  * [Usage](#usage)
  * [Reporting Bugs](#bugs)
    
***

### Installation ###

To install TradeSharp, follow the steps given [here](https://github.com/trade-nexus/tradesharp-core#installation)

### Usage ###

Once the TradeSharp has started, the dashboard page (see Figure 1 below) is loaded. Menu on the right allows the user to connect with multiple market data and order execution providers. On the left of the dashboard page, there are tabs for navigating between Dashboard, Strategy Runner, Orders and Data Downloader views. 

![TradeSharp Dashboard](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/dashboard.png)
Figure 1: TradeSharp Dashboard

To get live data from the market data provider for any symbol, create a new market scanner (if not already created) for that provider using the **New Market Scanner** on the dashboard view and add the symbols to the scanner. See Figure 2 below.

![TradeSharp Dashboard with Multiple Market Scanners](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/dashboard-with-scanner-windows.png)
Figure 2: TradeSharp Dashboard with Multiple Market Scanners

TradeSharp allows user to send orders manually using the entry order window. See Figure 3 below.

![TradeSharp Dashboard with Entry Order Window](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/dashboard-with-entry-order-window.png)
Figure 3: TradeSharp Dashboard with Entry Order Window

To execute an automated strategy, move to the strategy runner view by clicking on the second tab on the left of dashboard view. Load your strategy by clicking on the **Load Strategy** button and create or load and instance for that strategy using the **Create Strategy** or **Import Instances** button respectively. TradeSharp allows creating multiple instances of same strategy. See Figure 4 below of TradeSharp Strategy Runner view. 

![TradeSharp Strategy Runner](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/strategy-runner.png)
Figure 4: TradeSharp Strategy Runner

Use genetic or brute force optimization feature provided by TradeSharp to find the best parameters for your strategy. Figure 5 below shows the TradeSharp genetic and brute force optimization windows.

![TradeSharp Genetic and Brute Force Optimization](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/genetic-and-brute-optimization.png)
Figure 5: TradeSharp Genetic and Brute Force Optimization

To view the order statistics click on the third tab on the left of dashboard view. Figure 6 below shows TradeSharp order statistics view. 

![TradeSharp Order Statistics](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/order-statistics.png)
Figure 6: TradeSharp Order Statistics

TradeSharp allows downloading data from a provider to be used later for back testing. Click on the last tab on the left of dashboard view to see the Data Downloader view. See Figure 7 below.  

![TradeSharp Data Downloader](https://github.com/trade-nexus/tradesharp-screenshots/blob/master/data-downloader.png)
Figure 7: TradeSharp Data Downloader

### Bugs

Please report bugs [here](https://github.com/trade-nexus/bugs)
