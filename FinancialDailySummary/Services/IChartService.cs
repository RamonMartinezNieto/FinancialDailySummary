namespace FinancialDailySummary.Services;

public interface IChartService
{
    string GetUrlChart(string[] labels, int?[] data, CommandsEnum.Commands index);
}