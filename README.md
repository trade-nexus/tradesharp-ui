# Tradehub - UI #

This repository contains all the front-end code for 'TradeHub'.

### JIRA link: ###
https://aurorasolutions.atlassian.net

## Requirements ##

* MS Visual Studio 2012 or higher
* .Net Framework 4.5.1
* MySql
* Git
* Rabbit MQ

## Setting up the code ##
### Database ###
* Settings:
    * Username = `root`
    * Password = `root`
    * host = `localhost`

+ Create a new database named `TradeHub`.
+ Run sql script: `TradeHubDBScript.sql` located in `..\tradehub-ui\database\`
### Rabbit MQ###
+ Download and install erlang: http://www.erlang.org/download.html
+ Restart System.
+ Download and install RabbitMQ: http://www.rabbitmq.com/download.html
####Help#### 
If you run into any trouble for Rabbit MQ:
http://www.rabbitmq.com/install-windows.html

### Sample Data ###
Located in `..\tradehub-ui\Sample Data\`. Follow the instructions in `Read Me.txt` (as it will be updated depending on sample data added) located in the same folder to place the sample data.