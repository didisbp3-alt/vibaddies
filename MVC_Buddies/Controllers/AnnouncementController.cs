using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MVC_Buddies.Dtos;
using MVC_Buddies.Services;
using System.Net.Http;
using System.Security.Claims;

namespace MVC_Buddies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using MVC_Buddies.Dtos;

    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;
        private readonly HttpClient _http;

        public AnnouncementController(IAnnouncementService announcementService, IHttpClientFactory factory)
        {
            _announcementService = announcementService;
            _http = factory.CreateClient("API_Buddies"); // usado apenas para dropdowns e contas
        }

        // INDEX: lista de anúncios com filtros
        public async Task<IActionResult> Index(int? speciesId, int? serviceId, int? locationId)
        {
            List<AnnouncementListDto> announcements;

            if (speciesId.HasValue || serviceId.HasValue || locationId.HasValue)
            {
                // chama o service e faz o mapeamento
                var searchResults = await _announcementService
                    .SearchAnnouncementsAsync(speciesId, serviceId, locationId);

                announcements = searchResults.Select(a => new AnnouncementListDto
                {
                    Id = a.Id,
                    PetSitterFullName = a.PetSitterFullName,
                    LocationName = a.LocationName,
                    PricePerDay = a.PricePerDay,
                    AvatarUrl = a.AvatarUrl,
                    Services = a.Services
                }).ToList();
            }
            else
            {
                var all = await _announcementService.GetAllAnnouncementsAsync();
                // Mapear para AnnouncementListDto
                announcements = all.Select(a => new AnnouncementListDto
                {
                    Id = a.Id,
                    PetSitterFullName = a.PetSitterFullName,
                    LocationName = a.LocationName,
                    PricePerDay = a.PricePerDay,
                    AvatarUrl = a.AvatarUrl,
                    Services = a.Services
                }).ToList();
            }
            var model = new AnnouncementListViewModel
            {
                Announcements = announcements.Select(a => new AnnouncementSearchResultDto
                {
                    Id = a.Id,
                    PetSitterFullName = a.PetSitterFullName,
                    LocationName = a.LocationName,
                    PricePerDay = a.PricePerDay,
                    AvatarUrl = a.AvatarUrl,
                    Services = a.Services
                }).ToList(),

                SpeciesId = speciesId,
                ServiceId = serviceId,
                LocationId = locationId,

                Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new List<SpeciesDto>(),
                Services = await _http.GetFromJsonAsync<List<ServiceOptionDto>>("home/services") ?? new List<ServiceOptionDto>(),
                Location = await _http.GetFromJsonAsync<List<LocationDto>>("location") ?? new List<LocationDto>()
            };

            return View(model);
        }

        // Detalhes de um anúncio
        public async Task<IActionResult> Details(int id)
        {
            var announcement = await _announcementService.GetAnnouncementAsync(id);
            if (announcement == null)
                return NotFound();

            return View(announcement);
        }

        // Meus anúncios (Pet Sitter logado)
        public async Task<IActionResult> MyAnnouncements()
        {
            var petSitterId = await _http.GetFromJsonAsync<int>("accounts/me/petsitterid");

            if (petSitterId == 0)
                return RedirectToAction("Login", "Account");

            var announcements = await _http
                .GetFromJsonAsync<List<AnnouncementListDto>>($"petsitters/{petSitterId}/announcements");

            return View(announcements ?? new List<AnnouncementListDto>());
        }

        // Criar anúncio - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateAnnouncementDto
            {
                Locations = await _http.GetFromJsonAsync<List<LocationDto>>("location") ?? new List<LocationDto>(),
                Services = await _http.GetFromJsonAsync<List<ServiceDto>>("home/services") ?? new List<ServiceDto>(),
                Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new List<SpeciesDto>()
            };

            return View(model);
        }

        // Criar anúncio - POST
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementDto model)
        {
            if (!ModelState.IsValid)
            {
                // Recarrega dropdowns em caso de erro
                model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("location") ?? new List<LocationDto>();
                model.Services = await _http.GetFromJsonAsync<List<ServiceDto>>("home/services") ?? new List<ServiceDto>();
                model.Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new List<SpeciesDto>();
                return View(model);
            }

            var petSitterId = await _http.GetFromJsonAsync<int>("accounts/me/petsitterid");

            if (petSitterId == 0)
                return RedirectToAction("Login", "Account");

            var response = await _http.PostAsJsonAsync(
                $"petsitters/{petSitterId}/announcements",
                new
                {
                    model.LocationId,
                    SelectedServiceIds = model.ServiceIds,
                    SelectedSpeciesIds = model.SpeciesIds,
                    NewServices = model.NewServices,
                    NewSpecies = model.NewSpecies
                });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao criar anúncio");
                // Recarrega dropdowns
                model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("location") ?? new List<LocationDto>();
                model.Services = await _http.GetFromJsonAsync<List<ServiceDto>>("home/services") ?? new List<ServiceDto>();
                model.Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new List<SpeciesDto>();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // Deletar anúncio
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _http.DeleteAsync($"announcements/{id}");
            return RedirectToAction("MyAnnouncements");
        }
    }
}