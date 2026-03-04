using Microsoft.AspNetCore.WebUtilities;
using MVC_Buddies.Dtos;


namespace MVC_Buddies.Services
{
    public interface IAnnouncementService
    {
        Task<AnnouncementDetailsDto?> GetAnnouncementAsync(int id);
        Task<List<AnnouncementListDto>> GetAllAnnouncementsAsync();
        Task<List<ServiceDto>> GetAnnouncementServicesAsync(int announcementId);
        Task<List<AnnouncementSearchResultDto>> SearchAnnouncementsAsync(
            int? speciesId = null,
            int? serviceId = null,
            int? locationId = null);
    }

    public class AnnouncementService : IAnnouncementService
    {
        private readonly HttpClient _api;

        public AnnouncementService(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("API_Buddies");
        }

        public async Task<AnnouncementDetailsDto?> GetAnnouncementAsync(int id)
        {
            return await _api.GetFromJsonAsync<AnnouncementDetailsDto>($"api/announcements/{id}");
        }

        public async Task<List<AnnouncementListDto>> GetAllAnnouncementsAsync()
        {
            var result = await _api.GetFromJsonAsync<List<AnnouncementListDto>>("api/announcements");
            return result ?? new List<AnnouncementListDto>();
        }

        public async Task<List<ServiceDto>> GetAnnouncementServicesAsync(int announcementId)
        {
            var result = await _api.GetFromJsonAsync<List<ServiceDto>>($"api/announcements/{announcementId}/services");
            return result ?? new List<ServiceDto>();
        }

        public async Task<List<AnnouncementSearchResultDto>> SearchAnnouncementsAsync(
            int? speciesId = null,
            int? serviceId = null,
            int? locationId = null)
        {
            var qs = new List<string>();

            if (speciesId.HasValue) qs.Add($"speciesId={speciesId.Value}");
            if (serviceId.HasValue) qs.Add($"serviceId={serviceId.Value}");
            if (locationId.HasValue) qs.Add($"locationId={locationId.Value}");

            var url = "api/announcements/search" + (qs.Count > 0 ? "?" + string.Join("&", qs) : "");

            var result = await _api.GetFromJsonAsync<List<AnnouncementSearchResultDto>>(url);
            return result ?? new List<AnnouncementSearchResultDto>();
        }
    }
}
