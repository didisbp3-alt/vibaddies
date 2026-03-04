using MVC_Buddies.Dtos;

namespace MVC_Buddies.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto);
        Task<List<ReviewDto>> GetReviewsByBookingAsync(int bookingId);
        Task<List<ReviewDto>> GetReviewsByPetSitterAsync(int petSitterId);
        Task DeleteReviewAsync(int id);
    }

    public class ReviewService : IReviewService
    {
        private readonly HttpClient _api;

        public ReviewService(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("API_Buddies");
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto dto)
        {
            var response = await _api.PostAsJsonAsync("reviews", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReviewDto>()!;
        }

        public async Task<List<ReviewDto>> GetReviewsByBookingAsync(int bookingId)
        {
            var reviews = await _api.GetFromJsonAsync<List<ReviewDto>>($"reviews/booking/{bookingId}");
            return reviews ?? new List<ReviewDto>();
        }

        public async Task<List<ReviewDto>> GetReviewsByPetSitterAsync(int petSitterId)
        {
            var reviews = await _api.GetFromJsonAsync<List<ReviewDto>>($"reviews/petsitter/{petSitterId}");
            return reviews ?? new List<ReviewDto>();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var response = await _api.DeleteAsync($"reviews/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
