using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Buddies.Dtos;

namespace MVC_Buddies.Controllers
{
    public class AdminCRUDController : Controller
    {
        private readonly HttpClient _http;

        public AdminCRUDController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API_Buddies");
        }



        public async Task<IActionResult> Index()
        {
            return View();
        }
        // =====================================================
        // LIST
        // =====================================================



        public async Task<IActionResult> IndexBreed(int? speciesId)
        {
            // 1️⃣ Buscar todas as species
            var speciesList = await _http.GetFromJsonAsync<List<SpeciesDto>>("admin-crud/species");
            ViewBag.SpeciesList = speciesList;

            // 2️⃣ Buscar todas as breeds
            var breedDtos = await _http.GetFromJsonAsync<List<BreedItemDto>>("admin-crud/breeds");

            // 3️⃣ Converter para ViewModel e filtrar se necessário
            var viewModel = breedDtos
                .Where(b => !speciesId.HasValue || b.SpeciesId == speciesId.Value)
                .Select(b => new BreedViewModel
                {
                    Id = b.Id,
                    SpeciesId = b.SpeciesId,
                    Name = b.Name,
                    SpeciesName = speciesList.FirstOrDefault(s => s.Id == b.SpeciesId)?.Name ?? "Unknown"
                })
                .ToList();

            return View(viewModel);
        }


        // =====================================================
        // CREATE
        // =====================================================


        public async Task<IActionResult> CreateBreed()
        {
            var species = await _http.GetFromJsonAsync<List<RefItemDto>>("admin-crud/species");

            var vm = new BreedCreateViewModel
            {
                SpeciesList = species
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBreed(BreedCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new BreedCreateDto(vm.SpeciesId, vm.Name);


            var res = await _http.PostAsJsonAsync("admin-crud/breeds", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao criar a breed");
                return View(vm);
            }

            return RedirectToAction("IndexBreed");
        }



        // =====================================================
        // EDIT
        // =====================================================

        public async Task<IActionResult> EditBreed(int id)
        {
            var breed = await _http.GetFromJsonAsync<BreedItemDto>($"admin-crud/breeds/{id}");
            if (breed == null) return NotFound();

            var species = await _http.GetFromJsonAsync<List<RefItemDto>>("admin-crud/species");

            var vm = new BreedEditViewModel
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesId = breed.SpeciesId,
                SpeciesList = species
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditBreed(BreedEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new RefUpdateDto(vm.Name);
            var res = await _http.PutAsJsonAsync($"admin-crud/breeds/{vm.Id}", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao atualizar a breed");
                return View(vm);
            }

            return RedirectToAction("IndexBreed");
        }



        // =====================================================
        // DELETE
        // =====================================================

        public async Task<IActionResult> DeleteBreed(int id)
        {
            await _http.DeleteAsync($"admin-crud/breeds/{id}");
            return RedirectToAction(nameof(Index));
        }



        // =====================================================
        // Location
        // =====================================================
        public async Task<IActionResult> IndexLocation()
        {
            var locations = await _http
                .GetFromJsonAsync<List<LocationItemDto>>("admin-crud/locations");

            return View(locations);
        }

        public IActionResult CreateLocation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation(LocationCreateDto vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var res = await _http.PostAsJsonAsync("admin-crud/locations", vm);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao criar location");
                return View(vm);
            }

            return RedirectToAction(nameof(IndexLocation));
        }

        public async Task<IActionResult> EditLocation(int id)
        {
            var location = await _http.GetFromJsonAsync<LocationItemDto>($"admin-crud/locations/{id}");

            if (location == null) return NotFound();

            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationItemDto vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new LocationUpdateDto(vm.Name, vm.District, vm.Country);

            var res = await _http.PutAsJsonAsync($"admin-crud/locations/{vm.Id}", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao atualizar location");
                return View(vm);
            }

            return RedirectToAction(nameof(IndexLocation));
        }

        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _http.DeleteAsync($"admin-crud/locations/{id}");
            return RedirectToAction(nameof(IndexLocation));
        }

        // =====================================================
        //Services
        // =====================================================

        public async Task<IActionResult> IndexService()
        {
            var services = await _http.GetFromJsonAsync<List<ServiceItemDto>>("admin-crud/services");

            return View(services);
        }
        public IActionResult CreateService()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var res = await _http.PostAsJsonAsync("admin-crud/services", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao criar service");
                return View(dto);
            }

            return RedirectToAction(nameof(IndexService));
        }

        public async Task<IActionResult> EditService(int id)
        {
            var service = await _http.GetFromJsonAsync<ServiceItemDto>($"admin-crud/services/{id}");

            if (service == null) return NotFound();

            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(ServiceItemDto vm)
        {
            var dto = new RefUpdateDto(vm.Name);

            var res = await _http.PutAsJsonAsync($"admin-crud/services/{vm.Id}", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao atualizar");
                return View(vm);
            }

            return RedirectToAction(nameof(IndexService));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _http.DeleteAsync($"admin-crud/services/{id}");

            return RedirectToAction(nameof(IndexService));
        }

        // =====================================================
        // SKILLS
        // =====================================================

        public async Task<IActionResult> IndexSkill()
        {
            var skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills");

            return View(skills);
        }

        public IActionResult CreateSkill()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill(SkillCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var res = await _http.PostAsJsonAsync("admin-crud/skills", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao criar skill");
                return View(dto);
            }

            return RedirectToAction(nameof(IndexSkill));
        }
        public async Task<IActionResult> EditSkill(int id)
        {
            var skill = await _http.GetFromJsonAsync<SkillItemDto>($"admin-crud/skills/{id}");

            if (skill == null) return NotFound();

            return View(skill);
        }

        [HttpPost]
        public async Task<IActionResult> EditSkill(SkillItemDto vm)
        {
            var dto = new RefUpdateDto(vm.Name);

            var res = await _http.PutAsJsonAsync($"admin-crud/skills/{vm.Id}", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao atualizar skill");
                return View(vm);
            }

            return RedirectToAction(nameof(IndexSkill));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            await _http.DeleteAsync($"admin-crud/skills/{id}");

            return RedirectToAction(nameof(IndexSkill));
        }


        // =====================================================
        // SPECIES
        // =====================================================

        public async Task<IActionResult> IndexSpecies()
        {
            var species = await _http.GetFromJsonAsync<List<SpeciesItemDto>>("admin-crud/species");

            return View(species);
        }

        public IActionResult CreateSpecies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecies(SpeciesCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var res = await _http.PostAsJsonAsync("admin-crud/species", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao criar species");
                return View(dto);
            }

            return RedirectToAction(nameof(IndexSpecies));
        }

        public async Task<IActionResult> EditSpecies(int id)
        {
            var species = await _http.GetFromJsonAsync<SpeciesItemDto>($"admin-crud/species/{id}");

            if (species == null) return NotFound();

            return View(species);
        }

        [HttpPost]
        public async Task<IActionResult> EditSpecies(SpeciesItemDto vm)
        {
            var dto = new RefUpdateDto(vm.Name);

            var res = await _http.PutAsJsonAsync($"admin-crud/species/{vm.Id}", dto);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Erro ao atualizar species");
                return View(vm);
            }

            return RedirectToAction(nameof(IndexSpecies));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSpecies(int id)
        {
            await _http.DeleteAsync($"admin-crud/species/{id}");

            return RedirectToAction(nameof(IndexSpecies));
        }


        // =====================================================
        // PENDING SITTER 
        // =====================================================

        public async Task<IActionResult> PendingPetSitters()
        {
            var response = await _http.GetFromJsonAsync<List<PendingPetSitterViewModel>>("admin-crud/pending-petsitters");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _http.PutAsync($"admin-crud/approve-petsitter/{id}", null);
            return RedirectToAction("PendingPetSitters");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _http.DeleteAsync($"admin-crud/delete-petsitter/{id}");
            return RedirectToAction("PendingPetSitters");
        }
    }
}
