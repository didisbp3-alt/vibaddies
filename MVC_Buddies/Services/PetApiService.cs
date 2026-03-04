using System.Net.Http.Json;
using MVC_Buddies.Dtos;

namespace MVC_Buddies.Services
{
    public interface IPetService
    {
        Task<PetDto?> GetPetByIdAsync(int id);
        Task<List<PetDto>> GetMyPetsAsync();
        Task<int> CreatePetAsync(CreatePetDto dto);
        Task UpdatePetAsync(int id, UpdatePetDto dto);
        Task DeletePetAsync(int id);
        Task<List<BreedDto>> GetBreedsAsync();
        Task<List<BreedDto>> GetBreedsBySpeciesAsync(int speciesId);
    }

    public class PetService : IPetService
    {
        private readonly HttpClient _api;

        public PetService(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("API_Buddies");
        }

        public async Task<PetDto?> GetPetByIdAsync(int id)
        {
            return await _api.GetFromJsonAsync<PetDto>($"pet/{id}");
        }

        public async Task<List<PetDto>> GetMyPetsAsync()
        {
            var pets = await _api.GetFromJsonAsync<List<PetDto>>("pet");
            return pets ?? new List<PetDto>();
        }

        public async Task<int> CreatePetAsync(CreatePetDto dto)
        {
            var response = await _api.PostAsJsonAsync("pet", dto);
            response.EnsureSuccessStatusCode();

            // Retorna o Id criado
            var id = await response.Content.ReadFromJsonAsync<int>();
            return id;
        }

        public async Task UpdatePetAsync(int id, UpdatePetDto dto)
        {
            var response = await _api.PutAsJsonAsync($"pet/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePetAsync(int id)
        {
            var response = await _api.DeleteAsync($"pet/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<BreedDto>> GetBreedsAsync()
        {
            var breeds = await _api.GetFromJsonAsync<List<BreedDto>>("pet/breed");
            return breeds ?? new List<BreedDto>();
        }

        public async Task<List<BreedDto>> GetBreedsBySpeciesAsync(int speciesId)
        {
            var breeds = await _api.GetFromJsonAsync<List<BreedDto>>($"pet/breed/{speciesId}");
            return breeds ?? new List<BreedDto>();
        }
    }
}
