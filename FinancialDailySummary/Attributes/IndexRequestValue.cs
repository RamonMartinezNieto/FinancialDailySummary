namespace FinancialDailySummary.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal class IndexRequestValue : Attribute
{
    public string Value { get; set; }

    public IndexRequestValue(string value)
    {
        Value = value;
    }
}

internal static class IndexRequestValueExtension
{
    public static string GetIndexRequest(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attributes = field.GetCustomAttributes(typeof(IndexRequestValue), false) as IndexRequestValue[];
        return attributes != null && attributes.Length > 0 ? attributes[0].Value : value.ToString();
    }
}