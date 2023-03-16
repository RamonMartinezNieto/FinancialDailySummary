using Newtonsoft.Json;

namespace FinancialDailySummary.Services;

public class ChartService : IChartService
{
    private IMemoryCache _cache;

    private readonly int _cacheExpirationMinutes = 10;
    private readonly int _widthChart = 500;
    private readonly int _heightChart = 300;
    private readonly string _versionChart = "2.9.4";
    private readonly float _devicePixelRatio = 2.0f;

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

        var chart = GenerateChart(labels, data, index);
        var url = chart.GetShortUrl();

        _cache.Set(cacheKey, url, TimeSpan.FromMinutes(_cacheExpirationMinutes));
        return url;
    }

    private QuickChart.Chart GenerateChart(string[] labels,
        int?[] data,
        CommandsEnum.Commands index)
        => new()
        {
            Width = _widthChart,
            Height = _heightChart,
            Version = _versionChart,
            Config = GetLinearChartJson(labels, data, index),
            DevicePixelRatio = _devicePixelRatio
        };

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
