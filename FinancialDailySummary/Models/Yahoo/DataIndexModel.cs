﻿namespace FinancialDailySummary.Models.Yahoo;

public class DataIndexModel
{
    public Chart Chart { get; set; }

    public string GetLastValue() =>
        Chart.Result[0].Indicators.Quote[0].Close.Last().ToString();
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
    public string Wymbol { get; set; }
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
