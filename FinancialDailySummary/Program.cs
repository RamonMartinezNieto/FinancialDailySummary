namespace FinancialDailySummary;

internal class Program
{
    static void Main()
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>(true)
            .Build();

        IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
            services => {

            services.ConfigureBotSettings(
                Environment.GetEnvironmentVariable(
                    Configuration.GetSection("FinancialEnvironmentVariables:BotToken").Value));

            services.AddLogging();

            services.AddHttpClient<YahooFinancialClient>(
                c => {
                    c.BaseAddress = new Uri(Configuration.GetSection("FinancialClients:YahooFinacial").Value);
                    c.Timeout = TimeSpan.FromSeconds(30);
                });

            services.AddMemoryCache();

            services.AddScoped<IChartService, ChartService>();
            services.AddTransient<IFinancialClient<DataIndexModel>, YahooFinancialClient>();  
            services.AddSingleton<ITelegramBot,TelegramBot>();  
            services.AddSingleton<IApplication, Application>();

        })
        .Build();

        var app = _host.Services.GetRequiredService<IApplication>();
        app.Run();

        Thread.Sleep(Timeout.Infinite);
    }
}
