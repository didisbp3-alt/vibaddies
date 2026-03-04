using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Models;
using System.Security.Claims;

namespace MVC_Buddies.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private readonly HttpClient _http;
        private readonly IWebHostEnvironment _env;

        public AccountController(IHttpClientFactory factory, IWebHostEnvironment env)
        {
            _http = factory.CreateClient("API_Buddies");
            _env = env;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills");
            var species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species");
            var locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations");

            ViewBag.Locations = locations ?? new List<LocationDto>();
            ViewBag.Skills = skills ?? new List<SkillItemDto>();
            ViewBag.Species = species ?? new List<SpeciesDto>();

            return View();
        }

        // Ponto central de decisão após login/registo
        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _http.GetFromJsonAsync<AccountProfileDto>($"accounts/{userId}");

            if (profile == null)
            {
                return NotFound();
            }

            bool isComplete = IsProfileComplete(profile);

            if (isComplete)
            {
                return RedirectToAction("Index");
            }

            if (profile.RoleName == "PetSitter")
            {
                return RedirectToAction("CreatePetSitterProfile");
            }

            if (profile.RoleName == "PetOwner")
            {
                return RedirectToAction("CreatePetOwnerProfile");
            }

            // fallback raro
            return RedirectToAction("Index");
        }

        private bool IsProfileComplete(AccountProfileDto profile)
        {
            if (profile.RoleName == "PetSitter")
            {
                return
                    !string.IsNullOrWhiteSpace(profile.FullName) &&
                    !string.IsNullOrWhiteSpace(profile.PhotoUrl) &&
                    profile.YearsExperience.GetValueOrDefault() >= 0 &&
                    profile.LocationId.GetValueOrDefault() > 0 &&
                    (profile.SelectedSkillIds?.Any() ?? false) &&
                    (profile.SelectedSpeciesIds?.Any() ?? false);
            }

            if (profile.RoleName == "PetOwner")
            {
                return
                    !string.IsNullOrWhiteSpace(profile.FullName) &&
                    profile.LocationId.GetValueOrDefault() > 0;
        
            }

            return false;
        }

        [HttpGet]
        public async Task<IActionResult> CreatePetSitterProfile()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _http.GetFromJsonAsync<AccountProfileDto>($"accounts/{userId}");

            if (profile == null || profile.RoleName != "PetSitter")
                return RedirectToAction("CompleteProfile");

            if (IsProfileComplete(profile))
                return RedirectToAction("Index");

            var vm = new CreatePetSitterViewModel
            {
                Id = profile.UserId,
                FullName = profile.FullName,
                PhotoUrl = profile.PhotoUrl,
                YearsExperience = profile.YearsExperience,
                LocationId = profile.LocationId,
                SelectedSkillIds = profile.SelectedSkillIds ?? new(),
                SelectedSpeciesIds = profile.SelectedSpeciesIds ?? new(),
                Skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills") ?? new(),
                Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new(),
                Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePetSitterProfile(CreatePetSitterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills") ?? new();
                model.Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new();
                model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new();
                return View(model);
            }

            string? photoUrl = model.PhotoUrl;

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/profile-pictures");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = $"{Guid.NewGuid()}_{model.ProfilePicture.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await model.ProfilePicture.CopyToAsync(stream);

                    photoUrl = $"/uploads/profile-pictures/{fileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePicture", $"Erro ao guardar foto: {ex.Message}");
                    model.Skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills") ?? new();
                    model.Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new();
                    model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new();
                    return View(model);
                }
            }

            var updateDto = new
            {
                Id = model.Id,
                FullName = model.FullName,
                PhotoUrl = photoUrl,
                YearsExperience = model.YearsExperience,
                LocationId = model.LocationId,
                SelectedSkillIds = model.SelectedSkillIds,
                SelectedSpeciesIds = model.SelectedSpeciesIds
                // Bio, SubscriptionType, etc. — adiciona se existirem no ViewModel
            };

            var response = await _http.PutAsJsonAsync($"accounts/{model.Id}", updateDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Não foi possível completar o perfil.");
                model.Skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills") ?? new();
                model.Species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species") ?? new();
                model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreatePetOwnerProfile()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _http.GetFromJsonAsync<AccountProfileDto>($"accounts/{userId}");

            if (profile == null || profile.RoleName != "PetOwner")
                return RedirectToAction("CompleteProfile");

            if (IsProfileComplete(profile))
                return RedirectToAction("Index");

            var vm = new CreatePetOwnerViewModel
            {
                Id = profile.UserId,
                FullName = profile.FullName,
                PhotoUrl = profile.PhotoUrl,
                Email = profile.Email,
                Bio = profile.Bio,
                SelectedLocationId = profile.LocationId,
                Address = profile.Address,
                PostalCode = profile.PostalCode,
                PhoneNumber = profile.PhoneNumber,
                Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new(),
                Pets = await _http.GetFromJsonAsync<List<PetDto>>("pets/my-pets") ?? new()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePetOwnerProfile(CreatePetOwnerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new();
                model.Pets = await _http.GetFromJsonAsync<List<PetDto>>("pets/my-pets") ?? new();
                return View(model);
            }

            string? photoUrl = model.PhotoUrl;

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads/profile-pictures");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = $"{Guid.NewGuid()}_{model.ProfilePicture.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await model.ProfilePicture.CopyToAsync(stream);

                    photoUrl = $"/uploads/profile-pictures/{fileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePicture", $"Erro ao guardar foto: {ex.Message}");
                    model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new();
                    model.Pets = await _http.GetFromJsonAsync<List<PetDto>>("pets/my-pets") ?? new();
                    return View(model);
                }
            }

            var updateDto = new
            {
                Id = model.Id,
                FullName = model.FullName,
                PhotoUrl = photoUrl,
                Bio = model.Bio,
                LocationId = model.SelectedLocationId,
                Address = model.Address,
                PostalCode = model.PostalCode,
                PhoneNumber = model.PhoneNumber,
                SelectedPetIds = model.SelectedPetIds ?? new List<int>()
            };

            var response = await _http.PutAsJsonAsync($"accounts/{model.Id}", updateDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Não foi possível completar o perfil.");
                model.Locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations") ?? new();
                model.Pets = await _http.GetFromJsonAsync<List<PetDto>>("pets/my-pets") ?? new();
                return View(model);
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var profile = await _http.GetFromJsonAsync<AccountProfileDto>($"accounts/{userId}");

            if (profile == null)
                return NotFound();
            Console.WriteLine(profile.RoleName);
            if (profile.RoleName == "PetSitter")
            {
                var skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills");
                var species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species");
                var locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations");

                var vm = new EditPetSitterViewModel
                {
                    Id = profile.UserId,
                    UserName = profile.UserName,
                    Email = profile.Email,
                    FullName = profile.FullName,
                    PhotoUrl = profile.PhotoUrl,
                    YearsExperience = profile.YearsExperience,
                    LocationId = profile.LocationId,
                    SubscriptionType = profile.SubscriptionType,
                    SelectedSkillIds = profile.SelectedSkillIds,
                    SelectedSpeciesIds = profile.SelectedSpeciesIds,
                    Skills = skills ?? new(),
                    Species = species ?? new(),
                    Locations = locations ?? new()
                };

                return View("EditPetSitter", vm);
            }

            var ownerVm = new EditPetOwnerViewModel
            {
                Id = profile.UserId,
                UserName = profile.UserName,
                Email = profile.Email
            };

            return View("EditPetOwner", ownerVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPetSitterViewModel model)
        {
            var url = $"accounts/{model.Id}";
            Console.WriteLine("PUT URL: " + new Uri(_http.BaseAddress, url));
            var response = await _http.PutAsJsonAsync($"accounts/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var profile = await _http.GetFromJsonAsync<AccountProfileDto>($"accounts/{userId}");

            if (profile == null)
                return NotFound();

            // 🔥 DECIDE A VIEW AQUI
            if (profile.RoleName == "PetSitter")
            {
                var skills = await _http.GetFromJsonAsync<List<SkillItemDto>>("admin-crud/skills");
                var species = await _http.GetFromJsonAsync<List<SpeciesDto>>("species");
                var locations = await _http.GetFromJsonAsync<List<LocationDto>>("admin-crud/locations");

                var vm = new PetSitterProfileViewModel
                {
                    Id = profile.UserId,
                    UserName = profile.UserName,
                    Email = profile.Email,
                    FullName = profile.FullName,
                    PhotoUrl = profile.PhotoUrl,
                    YearsExperience = profile.YearsExperience,
                    SubscriptionType = profile.SubscriptionType,
                    LocationName = locations?
                        .FirstOrDefault(l => l.Id == profile.LocationId)?.Name,

                    Skills = skills?
                        .Where(s => profile.SelectedSkillIds.Contains(s.Id))
                        .Select(s => s.Name)
                        .ToList() ?? new(),

                    Species = species?
                        .Where(s => profile.SelectedSpeciesIds.Contains(s.Id))
                        .Select(s => s.Name)
                        .ToList() ?? new()
                };

                return View("IndexPetSitter", vm);
            }

            // 🔹 PET OWNER
            var ownerVm = new PetOwnerProfileViewModel
            {
                Id = profile.UserId,
                UserName = profile.UserName,
                Email = profile.Email
            };

            return View("IndexPetOwner", ownerVm);
        }
    }

}
