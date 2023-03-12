namespace FinancialDailySummary.ExtensionMethods;

internal static class EnumExtensions
{
    public static bool TryGetDescription(this Enum value, out string commandDescription)
    {
        commandDescription = string.Empty;

        var field = value.GetType().GetField(value.ToString());
        var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Length > 0) 
        {
            commandDescription = attributes[0].Description;
            return true;
        }

        return false;
    }    

}
