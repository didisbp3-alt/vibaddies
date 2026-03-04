using Microsoft.AspNetCore.WebUtilities;
using MVC_Buddies.Dtos;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace MVC_Buddies.Services
{


    public interface IHomeApiService
    {
        Task<MainViewModel> GetMainAsync(string? location, int take = 8);
         Task<List<ServiceDto>> GetServicesAsync();
        Task<MainViewModel> SearchAsync(string? location, DateTime? from, DateTime? to, int? serviceId);
    }



    public class HomeApiService : IHomeApiService
    {
        private readonly HttpClient _api;

        public HomeApiService(HttpClient api)
        {
            _api = api;
        }


        public async Task<MainViewModel> GetMainAsync(string? location, int take = 8)
        {
            var loc = location?.Trim();
          
         
            var url = loc != null
                ? $"home/main?location={Uri.EscapeDataString(loc)}&take={take}"
                : $"home/main?take={take}";


          

            var dto = await _api.GetFromJsonAsync<HomeMainDto>(url);

            if (dto is null)
            {
                return new MainViewModel
                {
                    Location = loc,
                    Sitters = new List<SitterCardDto>()
                };
            }

            return new MainViewModel
            {
                Location = dto.Location,
                Sitters = dto.Sitters.Select(s => new SitterCardDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    City = s.Location,
                    AvatarUrl = s.AvatarUrl,
                    PricePerDay = s.PricePerDay,
                    Rating = s.Rating,
                    ReviewsCount = s.ReviewsCount,
                    Services = s.Services
                }).ToList()
            };
        }



     

        public async Task<List<ServiceDto>> GetServicesAsync()
        {
            try
            {
                var response = await _api.GetAsync("home/services");

                Console.WriteLine("STATUS: " + response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("CONTENT: " + content);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<ServiceDto>>() ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO REAL: " + ex.ToString());
                throw;
            }
        }




        public async Task<MainViewModel> SearchAsync(
        string? location,
        DateTime? from,
        DateTime? to,
        int? serviceId)
        {
            var query = new Dictionary<string, string?>()
            {
                ["location"] = location,
                ["from"] = from?.ToString("yyyy-MM-dd"),
                ["to"] = to?.ToString("yyyy-MM-dd"),
                ["serviceId"] = serviceId?.ToString()
            };

            var url = QueryHelpers.AddQueryString("home/search", query!);

     //       var sitters =
     //await _api.GetFromJsonAsync<List<SitterCardDto>>(url)
     //?? new();
            var rawSitters = await _api.GetFromJsonAsync<List<SearchSitterDto>>(url) ?? new();

            var sitters = rawSitters
              .GroupBy(x => new { x.PetSitterId, x.FullName, x.City })
              .Select(g => new SitterCardDto
              {
                  Id = g.Key.PetSitterId,
                  Name = g.Key.FullName,
                  City = g.Key.City,
                  AvatarUrl = "",
                  PricePerDay = 0,
                  Rating = 0,
                  ReviewsCount = 0,
                  AnnouncementIds = g.SelectMany(x => x.AnnouncementId).Distinct().ToList()
              })
              .ToList();

            return new MainViewModel
            {
                Location = location ?? "Lisboa",
                Sitters = sitters
            };
        }



    }


}
