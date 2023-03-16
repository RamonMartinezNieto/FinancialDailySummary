using Newtonsoft.Json;

namespace FinancialDailySummary.Services;

public class ChartService : IChartService
{
    private IMemoryCache _cache;

    private QuickChart.Chart _chart;

    public ChartService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public string GetUrlChart(string[] labels,
        int?[] data,
        CommandsEnum.Commands index)
    {
        var cacheKey = $"{index}UriChart";

        if (_cache.TryGetValue(cacheKey, out string uriChart))
            return uriChart;

        _chart = GenerateChart(labels, data, index);
        var url = _chart.GetShortUrl();

        _cache.Set(cacheKey, url, TimeSpan.FromMinutes(5));
        return url;
    }

    private QuickChart.Chart GenerateChart(string[] labels,
        int?[] data,
        CommandsEnum.Commands index)
    {
        QuickChart.Chart chart = new QuickChart.Chart();
        chart = new();
        chart.Width = 500;
        chart.Height = 300;
        chart.Version = "2.9.4";
        chart.Config = GetLinearChartJson(labels, data, index);
        chart.DevicePixelRatio = 2.0;

        return chart;
    }

    private static string GetLinearChartJson(string[] labels, int?[] data, CommandsEnum.Commands index)
    {
        var linearChart = new
        {
            type = "line",
            data = new
            {
                labels = labels,
                datasets = new[]
                {
                     new
                     {
                         label = index.ToString(),
                         data = data,
                         fill = false,
                         borderColor = "blue"
                     }
                 }
            },
            options = new
            {
                responsive = true,
                legend = false,
                title = new
                {
                    text = index.ToString(),
                    display = true
                },
                scales = new
                {
                    yAxes = new[]
                    {
                        new
                        {
                            ticks = new
                            {
                                suggestedMin = (int)data.Min() - 5,
                                suggestedMax = (int)data.Max() + 5
                            }
                        }
                    }
                }
            }
        };

        return JsonConvert.SerializeObject(linearChart);
    }
}
