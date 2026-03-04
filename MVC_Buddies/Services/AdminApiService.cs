using MVC_Buddies.Dtos;
using System.Net.Http.Json;
namespace MVC_Buddies.Services
{

    public interface IAdminApiService
    {
        Task<AdminKpisDto?> GetKpisAsync();

        Task<PagedResult<AdminUserListItemDto>?> GetUsersAsync(
            string? search, string? role, bool? active, int page, int pageSize);

        Task<bool> UpdateUserStatusAsync(int userId, bool isActive);

        Task<PagedResult<PendingPetSitterItemVm>?> GetPendingPetSittersAsync(int page, int pageSize);
        Task<bool> ApprovePetSitterAsync(int petSitterId, bool isApproved);

        Task<PagedResult<AnnouncementAdminItemVm>?> GetAnnouncementsAsync(bool? active, int page, int pageSize);
        Task<bool> SetAnnouncementActiveAsync(int announcementId, bool isActive);
    }

    public class AdminApiService : IAdminApiService
    {
        private readonly HttpClient _api;

        public AdminApiService(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("API_Buddies");
        }

        public Task<AdminKpisDto?> GetKpisAsync()
            => _api.GetFromJsonAsync<AdminKpisDto>("api/admin/kpis");

        public async Task<PagedResult<AdminUserListItemDto>?> GetUsersAsync(
            string? search, string? role, bool? active, int page, int pageSize)
        {
            var qs = new List<string>();
            if (!string.IsNullOrWhiteSpace(search)) qs.Add($"search={Uri.EscapeDataString(search)}");
            if (!string.IsNullOrWhiteSpace(role)) qs.Add($"role={Uri.EscapeDataString(role)}");
            if (active.HasValue) qs.Add($"active={active.Value.ToString().ToLowerInvariant()}");
            qs.Add($"page={page}");
            qs.Add($"pageSize={pageSize}");

            var url = "api/admin/users" + (qs.Count > 0 ? "?" + string.Join("&", qs) : "");
            return await _api.GetFromJsonAsync<PagedResult<AdminUserListItemDto>>(url);
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isActive)
        {
            var res = await _api.PatchAsJsonAsync($"api/admin/users/{userId}/status", new UpdateUserStatusDto(isActive));
            return res.IsSuccessStatusCode;
        }

        public async Task<PagedResult<PendingPetSitterItemVm>?> GetPendingPetSittersAsync(int page, int pageSize)
        {
            var url = $"api/admin/petsitters/pending?page={page}&pageSize={pageSize}";
            return await _api.GetFromJsonAsync<PagedResult<PendingPetSitterItemVm>>(url);
        }

        public async Task<bool> ApprovePetSitterAsync(int petSitterId, bool isApproved)
        {
            var res = await _api.PostAsJsonAsync($"api/admin/petsitters/{petSitterId}/approve", new ApprovePetSitterDto(isApproved));
            return res.IsSuccessStatusCode;
        }

        public async Task<PagedResult<AnnouncementAdminItemVm>?> GetAnnouncementsAsync(bool? active, int page, int pageSize)
        {
            var qs = new List<string>();
            if (active.HasValue) qs.Add($"active={active.Value.ToString().ToLowerInvariant()}");
            qs.Add($"page={page}");
            qs.Add($"pageSize={pageSize}");

            var url = "api/admin/announcements?" + string.Join("&", qs);
            return await _api.GetFromJsonAsync<PagedResult<AnnouncementAdminItemVm>>(url);
        }

        public async Task<bool> SetAnnouncementActiveAsync(int announcementId, bool isActive)
        {
            var res = await _api.PostAsJsonAsync($"api/admin/announcements/{announcementId}/set-active",
                new SetAnnouncementActiveDto(isActive));

            return res.IsSuccessStatusCode;
        }
    }

}
