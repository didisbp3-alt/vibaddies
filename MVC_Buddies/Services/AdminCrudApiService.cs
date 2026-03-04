
using MVC_Buddies.Dtos;
using System.Net.Http.Json;
namespace MVC_Buddies.Services
{
    public interface IAdminCrudApiService
    {
        //Species
       Task<List<RefItemDto>> GetSpeciesAsync();
        Task<RefItemDto?> CreateSpeciesAsync(string name);
        Task<bool> UpdateSpeciesAsync(int id, string name);
        Task<bool> DeleteSpeciesAsync(int id);

       // Breeds
       Task<List<BreedItemDto>> GetBreedsAsync(int speciesId);
        Task<BreedItemDto?> CreateBreedAsync(int speciesId, string name);
        Task<bool> UpdateBreedAsync(int id, string name);
        Task<bool> DeleteBreedAsync(int id);

        //Skills
       Task<List<RefItemDto>> GetSkillsAsync();
        Task<RefItemDto?> CreateSkillAsync(string name);
        Task<bool> UpdateSkillAsync(int id, string name);
        Task<bool> DeleteSkillAsync(int id);
    }

    public class AdminCrudApiService : IAdminCrudApiService
    {
        private readonly HttpClient _api;

        public AdminCrudApiService(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("API_Buddies");
        }


        private const string BasePath = "api/adminCRUD";
        //Species
        public Task<List<RefItemDto>> GetSpeciesAsync()
            => _api.GetFromJsonAsync<List<RefItemDto>>($"{BasePath}/species") ?? Task.FromResult(new List<RefItemDto>());

        public async Task<RefItemDto?> CreateSpeciesAsync(string name)
        {
            var res = await _api.PostAsJsonAsync($"{BasePath}/species/create", new RefCreateDto(name));
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<RefItemDto>();
        }

        public async Task<bool> UpdateSpeciesAsync(int id, string name)
            => (await _api.PutAsJsonAsync($"{BasePath}/species/{id}", new RefUpdateDto(name))).IsSuccessStatusCode;

        public async Task<bool> DeleteSpeciesAsync(int id)
            => (await _api.DeleteAsync($"{BasePath}/species/{id}")).IsSuccessStatusCode;

        //Breeds
        public Task<List<BreedItemDto>> GetBreedsAsync(int speciesId)
            => _api.GetFromJsonAsync<List<BreedItemDto>>($"{BasePath}/breeds?speciesId={speciesId}") ?? Task.FromResult(new List<BreedItemDto>());

        //public async Task<BreedItemDto?> CreateBreedAsync(int speciesId, string name)
        //{
        //    var res = await _api.PostAsJsonAsync($"{BasePath}/breeds/create", new BreedCreateDto(speciesId, name));
        //    if (!res.IsSuccessStatusCode) return null;
        //    return await res.Content.ReadFromJsonAsync<BreedItemDto>();
        //}
        public async Task<BreedItemDto?> CreateBreedAsync(int speciesId, string name)
        {
            var res = await _api.PostAsJsonAsync(
                $"{BasePath}/breeds",
                new BreedCreateDto(speciesId, name)
            );

            if (!res.IsSuccessStatusCode) return null;

            return await res.Content.ReadFromJsonAsync<BreedItemDto>();
        }


        public async Task<bool> UpdateBreedAsync(int id, string name)
            => (await _api.PutAsJsonAsync($"{BasePath}/breeds/{id}", new RefUpdateDto(name))).IsSuccessStatusCode;

        public async Task<bool> DeleteBreedAsync(int id)
            => (await _api.DeleteAsync($"{BasePath}/breeds/{id}")).IsSuccessStatusCode;

        //Skills
        public Task<List<RefItemDto>> GetSkillsAsync()
            => _api.GetFromJsonAsync<List<RefItemDto>>($"{BasePath}/skills") ?? Task.FromResult(new List<RefItemDto>());

        public async Task<RefItemDto?> CreateSkillAsync(string name)
        {
            var res = await _api.PostAsJsonAsync($"{BasePath}/skills/create", new RefCreateDto(name));
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<RefItemDto>();
        }

        public async Task<bool> UpdateSkillAsync(int id, string name)
            => (await _api.PutAsJsonAsync($"{BasePath}/skills/{id}", new RefUpdateDto(name))).IsSuccessStatusCode;

        public async Task<bool> DeleteSkillAsync(int id)
            => (await _api.DeleteAsync($"{BasePath}/skills/{id}")).IsSuccessStatusCode;
    }

}
