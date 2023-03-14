
using Microsoft.Extensions.Caching.Memory;

namespace FinancialDailySummary.Clients;

internal class YahooFinancialClient : IFinancialClient<DataIndexModel>
{
    private readonly HttpClient _client;

    private readonly ILogger<YahooFinancialClient> _logger;
    private readonly IMemoryCache _cache;

    public YahooFinancialClient(
        HttpClient client,
        ILogger<YahooFinancialClient> logger,
        IMemoryCache cache)
    {
        _client = client;
        _logger = logger;
        _cache = cache;
    }

    public async Task<DataIndexModel> GetDataIndex(CommandsEnum.Commands index, Intervals interval)
    {
        try {

            if (_cache.TryGetValue(index, out var data)) 
                return (DataIndexModel)data;

            Uri uri = GetUri(index.GetIndexRequest(), interval);

            HttpResponseMessage response = await _client.GetAsync(uri);

            if(response != null && response.IsSuccessStatusCode) 
            {
                DataIndexModel formatResponse = await response.Content.ReadFromJsonAsync<DataIndexModel>();
                _cache.Set<DataIndexModel>(index, formatResponse, TimeSpan.FromMinutes(5));
                return formatResponse;
            }

            if (response != null)
                _logger.LogWarning($"Bad response StausCode: {response.StatusCode}");

            return new DataIndexModel();

        } catch (Exception ex)
        {
            _logger.LogCritical("Something was wrong!!!", ex);
            throw new Exception("Error getting DataIndexModel using YahooFinancialClient.GetDataIndex");
        }
    }

    private Uri GetUri(string index, Intervals param)
        => new (string.Format("{0}/{1}?interval={2}", 
            _client.BaseAddress, 
            index,
            ParamInterval(param)));

    readonly Func<Intervals, string> ParamInterval = (param) =>
        param.TryGetDescription(out string paramDescription) ? paramDescription : string.Empty;

}