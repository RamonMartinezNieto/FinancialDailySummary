*Financial Daily Summary*

This project involves connecting a bot with financial data to create a system for checking stocks in real-time.

Firstly, you need to create a Telegram bot using BotFather and obtain its token.

Documentation: https://core.telegram.org/bots/tutorial#obtain-your-bot-token

Next, add your BotToken to the secrets.json 

To add your custom secrets.json you need to follow next steps: 

1 - remove <UserSecretsId>3d9c90a9-cb8e-448a-976f-8de41b31bab8</UserSecretsId> from csproj
2 - dotnet user-secrets init  to create your secrets.json
3 - edit your secrets.json an add
    {
      "BotSettings": {
        "BotName": "Name of your bot",
        "BotToken": "Token of your bot"
      }
    }
