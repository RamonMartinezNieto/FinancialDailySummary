namespace FinancialDailySummary.Clients;

internal class YahooFinancialClient : IFinancialClient<DataIndexModel>
{
    private HttpClient _client;

    private ILogger<YahooFinancialClient> _logger;

    public YahooFinancialClient(
        HttpClient client,
        ILogger<YahooFinancialClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<DataIndexModel> GetDataIndex(CommandsEnum.Commands index, Intervals interval)
    {
        try { 
            Uri uri = GetUri(index.GetIndexRequest(), interval);

            HttpResponseMessage response = await _client.GetAsync(uri);

            if(response != null && response.IsSuccessStatusCode) 
                return await response.Content.ReadFromJsonAsync<DataIndexModel>();

            if(response != null)
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

    Func<Intervals, string> ParamInterval = (param) =>
        param.TryGetDescription(out string paramDescription) ? paramDescription : string.Empty;

}