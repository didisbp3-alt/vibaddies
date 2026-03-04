using MVC_Buddies.Dtos;

namespace MVC_Buddies.Services
{
    public interface IReportsApiService
    {
        Task<ProfitReportViewModel> GetProfitReportAsync(int year);
    }

    public class ReportsApiService : IReportsApiService
    {
        private readonly HttpClient _httpClient;

        public ReportsApiService(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient("API_Buddies");
        }

        public async Task<ProfitReportViewModel> GetProfitReportAsync(int year)
        {
            // Histórico
            var historyResponse = await _httpClient.GetAsync($"reports/profits/history?year={year}");
            var history = await historyResponse.Content.ReadFromJsonAsync<List<ProfitHistoryDto>>() ?? new();

            // Sumário
            var summaryResponse = await _httpClient.GetAsync($"reports/profits/summary?year={year}");
            var summary = await summaryResponse.Content.ReadFromJsonAsync<ProfitSummaryDto>() ?? new ProfitSummaryDto();

            // Anos disponíveis
            var availableYears = Enumerable.Range(DateTime.Now.Year - 5, 6).Reverse().ToList();

            return new ProfitReportViewModel
            {
                Year = year,
                Summary = summary,
                History = history,
                AvailableYears = availableYears
            };
        }
    }
}
