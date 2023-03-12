namespace FinancialDailySummary.Enums;

public static class CommandsEnum {

    internal enum Commands
    {
        [Description("/ibex35")]
        [IndexRequestValue("%5EIBEX")]
        Ibex35,        
        [Description("/sp500")]
        [IndexRequestValue("%5EGSPC")]
        SP500,
    }

    public static string[] GetListCommands()
        => Enum.GetValues(typeof(Commands))
               .Cast<Commands>()
               .Select(x => {
                   return x.TryGetDescription(out string commandDescription) 
                           ? commandDescription 
                           : string.Empty;
               })
               .ToArray();


    public static bool IsValidCommand(string text)
        => GetListCommands().Contains(text);


    public static string GetMessageListCommands(string header = "")
    {
        StringBuilder builder = new();
        
        if(!string.IsNullOrEmpty(header))  
            builder.AppendLine(header);

        builder.Append(Environment.NewLine);
        builder.AppendLine(string.Join(Environment.NewLine, CommandsEnum.GetListCommands()));
        return builder.ToString();
    }

    internal static Commands ParseEnumFromDescription(string messageCommand)
    {
        foreach (Commands value in Enum.GetValues(typeof(Commands)))
        {
            if (value.TryGetDescription(out string description)
                && description.Equals(messageCommand)) 
            {
                return value;
            }
        }

        throw new ArgumentException($"No se pudo convertir el literal '{messageCommand}' en un valor de Commands.");
    }
}