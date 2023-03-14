namespace FinancialDailySummary.Models;

public class LinearChart
{
    public string type { get; set; }
    public Data data { get; set; } = new();
    public Options options { get; set; } = new();
}

public class Data
{
    public string[] labels { get; set; }
    public Dataset[] datasets { get; set; }
}

public class Dataset
{
    public string label { get; set; }
    public int?[] data { get; set; }
    public bool fill { get; set; }
    public string borderColor { get; set; }
}

public class Options
{
    public bool responsive { get; set; }
    public Title title { get; set; } = new();
    public Scales scales { get; set; } = new();
    public bool legend { get; set; }
}

public class Title
{
    public bool display { get; set; }
    public string text { get; set; }
}

public class Scales
{
    public Yax[] yAxes { get; set; }
}

public class Yax
{
    public Ticks ticks { get; set; } = new();
}

public class Ticks
{
    public int suggestedMin { get; set; }
    public int suggestedMax { get; set; }
}