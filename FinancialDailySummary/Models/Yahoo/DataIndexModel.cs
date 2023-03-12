namespace FinancialDailySummary.Models.Yahoo;

public class DataIndexModel
{
    public Chart chart { get; set; }

    public string GetLastValue() =>
        chart.result[0].indicators.quote[0].close.Last().ToString();
}

public class Chart
{
    public Result[] result { get; set; }
    public object error { get; set; }
}

public class Result
{
    public Meta meta { get; set; }
    public long[] timestamp { get; set; }
    public Indicators indicators { get; set; }
}

public class Meta
{
    public string currency { get; set; }
    public string symbol { get; set; }
    public string exchangeName { get; set; }
    public string instrumentType { get; set; }
    public int firstTradeDate { get; set; }
    public int regularMarketTime { get; set; }
    public int gmtoffset { get; set; }
    public string timezone { get; set; }
    public string exchangeTimezoneName { get; set; }
    public float regularMarketPrice { get; set; }
    public float chartPreviousClose { get; set; }
    public float? previousClose { get; set; } //no ta
    public int? scale { get; set; } //no ta
    public int priceHint { get; set; }
    public Currenttradingperiod currentTradingPeriod { get; set; }
    public Tradingperiod?[][] tradingPeriods { get; set; }  //no ta
    public string dataGranularity { get; set; }
    public string range { get; set; }
    public string[] validRanges { get; set; }
}

public class Currenttradingperiod
{
    public Pre pre { get; set; }
    public Regular regular { get; set; }
    public Post post { get; set; }
}

public class SharedData 
{
    public string timezone { get; set; }
    public int end { get; set; }
    public int start { get; set; }
    public int gmtoffset { get; set; }
}

public class Pre : SharedData
{
}

public class Regular : SharedData
{
}

public class Post : SharedData
{
}

public class Tradingperiod : SharedData
{
}

public class Indicators
{
    public Quote[] quote { get; set; }
    public Adjclose?[] adjclose { get; set; }
}

public class Quote
{
    public float?[] open { get; set; }
    public float?[] volume { get; set; }
    public float?[] close { get; set; }
    public float?[] low { get; set; }
    public float?[] high { get; set; }
}

public class Adjclose
{
    public float[] adjclose { get; set; }
}
