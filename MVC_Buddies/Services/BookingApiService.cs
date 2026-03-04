using MVC_Buddies.Dtos;

namespace MVC_Buddies.Services
{
    public interface IBookingService
    {
        Task<List<PetOptionDto>> GetUserPetsAsync(int userId);
        Task<AnnouncementDetailsDto?> GetAnnouncementAsync(int announcementId);
        Task<List<SkillOptionDto>> GetPetsitterSkillsAsync(int petsitterId);
        Task<List<ServiceOptionDto>> GetAnnouncementServicesAsync(int announcementId);
        Task<int> GetPetOwnerIdAsync();
        Task<List<BookingListDto>> GetMyBookingsAsync();
        Task<HttpResponseMessage> CreateBookingAsync(CreateBookingDto dto);
        Task AcceptBookingAsync(int bookingId);
        Task RejectBookingAsync(int bookingId);
    }

    public class BookingService : IBookingService
    {
        private readonly HttpClient _http;

        public BookingService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PetOptionDto>> GetUserPetsAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<PetOptionDto>>($"petowner/{userId}/pets")
                   ?? new List<PetOptionDto>();
        }

        public async Task<AnnouncementDetailsDto?> GetAnnouncementAsync(int announcementId)
        {
            return await _http.GetFromJsonAsync<AnnouncementDetailsDto>($"announcements/{announcementId}");
        }

        public async Task<List<SkillOptionDto>> GetPetsitterSkillsAsync(int petsitterId)
        {
            return await _http.GetFromJsonAsync<List<SkillOptionDto>>($"petsitters/{petsitterId}/skills")
                   ?? new List<SkillOptionDto>();
        }

        public async Task<List<ServiceOptionDto>> GetAnnouncementServicesAsync(int announcementId)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<ServiceOptionDto>>($"announcements/{announcementId}/services")
                       ?? new List<ServiceOptionDto>();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<ServiceOptionDto>();
            }
        }

        public async Task<int> GetPetOwnerIdAsync()
        {
            return await _http.GetFromJsonAsync<int>("accounts/me/petownerid");
        }

        public async Task<List<BookingListDto>> GetMyBookingsAsync()
        {
            return await _http.GetFromJsonAsync<List<BookingListDto>>("bookings/my")
                   ?? new List<BookingListDto>();
        }

        public async Task<HttpResponseMessage> CreateBookingAsync(CreateBookingDto dto)
        {
            return await _http.PostAsJsonAsync("bookings", dto);
        }

        public async Task AcceptBookingAsync(int bookingId)
        {
            await _http.PutAsync($"bookings/{bookingId}/accept", null);
        }

        public async Task RejectBookingAsync(int bookingId)
        {
            await _http.PutAsync($"bookings/{bookingId}/reject", null);
        }
    }
}
