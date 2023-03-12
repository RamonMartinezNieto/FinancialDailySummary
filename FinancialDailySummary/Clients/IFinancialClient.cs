namespace FinancialDailySummary.Clients;

internal interface IFinancialClient<OutResult>
{
    Task<OutResult> GetDataIndex(CommandsEnum.Commands index, Intervals interval);
}
