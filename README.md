## *Financial Daily Summary*

This project involves connecting a bot with financial data to create a system for checking stocks in real-time.

### Bot creation 

Firstly, you need to create a Telegram bot using BotFather and obtain its token.

Steps: 
1. Found BotFather on telegram 
2. Use /newbot command
3. Follow the instructions 
4. Copy the token and the bot name. 

Documentation: https://core.telegram.org/bots/tutorial#obtain-your-bot-token

### Add enviroment variable name

Next, add your BotToken as an environment variable

1. In the appsettings.json set the name of your environment variable where is the token
{
  //....
  },
  "FinancialEnvironmentVariables": {
    "BotToken": "NAME_OF_YOUR_VARIABLE"
  }
}

### Remember

The bot is not deployed yet, in the future I will share it.

This is a side project just for practice.