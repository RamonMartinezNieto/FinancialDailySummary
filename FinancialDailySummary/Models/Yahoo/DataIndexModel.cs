using System.Globalization;

namespace FinancialDailySummary.Models.Yahoo;

public class DataIndexModel
{
    public Chart Chart { get; set; }

    private string GetLastCloseValue() =>
        GetFormatedSttring(Chart.Result[0].Indicators.Quote[0].Close.Last());

    private string GetPreviousClose() => 
        GetFormatedSttring(Chart.Result[0].Meta.ChartPreviousClose);

    private static string GetFormatedSttring(float? toFormat, string prefix = "")
    {
        if (toFormat.HasValue)
        {
            var culture = CultureInfo.GetCultureInfo("es-ES");
            string sign = toFormat.Value < 0 ? "\\-" : "";
            float absValue = Math.Abs(toFormat.Value);
            var formattedValue = $"{prefix} {sign}{absValue.ToString("0.00", culture)}".Replace(".", "");
            return formattedValue;
        }
        return null;
    }

    public string GetMessage(CommandsEnum.Commands index)
    {
        StringBuilder builder = new();

        CultureInfo culture = new ("es-ES");
        string date = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss", culture);

        builder.AppendLine(culture.TextInfo.ToTitleCase(date));
        builder.AppendLine($"*{ index.ToString().PadLeft(30) }*");
        builder.AppendLine(string.Format("{0,-30}{1}", "Previous:", GetPreviousClose()));
        builder.AppendLine(string.Format("{0,-31}{1}", "Current:", GetLastCloseValue()));
        builder.AppendLine(string.Format("{0,-30}{1}%", "Variaton:", GetDiffPercentil()));

        return builder.ToString();
    }

    private string GetDiffPercentil() 
    {
        var initialValue = Chart.Result[0].Meta.ChartPreviousClose;
        var diff = Chart.Result[0].Indicators.Quote[0].Close.Last() - initialValue;
        var percentile = (diff / initialValue) * 100;

        string prefix = percentile > 0 ? "▲" : "▼";
        return GetFormatedSttring(percentile, prefix);
    }
}

public class Chart
{
    public Result[] Result { get; set; }
    public object Error { get; set; }
}

public class Result
{
    public Meta Meta { get; set; }
    public long[] Timestamp { get; set; }
    public Indicators Indicators { get; set; }
}

public class Meta
{
    public string Currency { get; set; }
    public string Symbol { get; set; }
    public string ExchangeName { get; set; }
    public string InstrumentType { get; set; }
    public int FirstTradeDate { get; set; }
    public int RegularMarketTime { get; set; }
    public int Gmtoffset { get; set; }
    public string Timezone { get; set; }
    public string ExchangeTimezoneName { get; set; }
    public float RegularMarketPrice { get; set; }
    public float ChartPreviousClose { get; set; }
    public float? PreviousClose { get; set; }
    public int? Scale { get; set; }
    public int PriceHint { get; set; }
    public Currenttradingperiod CurrentTradingPeriod { get; set; }
    public TimeData[][] TradingPeriods { get; set; }
    public string DataGranularity { get; set; }
    public string Range { get; set; }
    public string[] ValidRanges { get; set; }
}

public class Currenttradingperiod
{
    public TimeData Pre { get; set; }
    public TimeData Regular { get; set; }
    public TimeData Post { get; set; }
}

public class TimeData 
{
    public string Timezone { get; set; }
    public int End { get; set; }
    public int Start { get; set; }
    public int Gmtoffset { get; set; }
}

public class Indicators
{
    public Quote[] Quote { get; set; }
    public Adjclose[] Adjclose { get; set; }
}

public class Quote
{
    public float?[] Open { get; set; }
    public float?[] Volume { get; set; }
    public float?[] Close { get; set; }
    public float?[] Low { get; set; }
    public float?[] High { get; set; }
}

public class Adjclose
{
    public float[] adjclose { get; set; }
}
