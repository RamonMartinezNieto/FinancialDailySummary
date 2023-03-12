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

### Add secrets.json

Next, add your BotToken to the secrets.json 

To add your custom secrets.json you need to follow next steps: 

1. remove < UserSecretsId > 3d9c90a9-cb8e-448a-976f-8de41b31bab8 < /UserSecretsId > from csproj
2. To create your own secrets.json use: dotnet user-secrets init  
3. Edit your secrets.json an add your bot's params
    {
      "BotSettings": {
        "BotName": "Name of your bot",
        "BotToken": "Token of your bot"
      }
    }

### Remember

The bot is not deployed yet, in the future I will share it.

This is a side project just for practice.