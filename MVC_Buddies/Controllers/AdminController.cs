using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Services;

namespace MVC_Buddies.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminApiService _admin;
        private readonly IAdminCrudApiService _crud;

        public AdminController(IAdminApiService admin, IAdminCrudApiService crud)
        {
            _admin = admin;
            _crud = crud;
        }

        // Dashboard 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kpis = await _admin.GetKpisAsync();
            return View(new AdminDashboardVm { Kpis = kpis });
        }

        // Users 
        [HttpGet]
        public async Task<IActionResult> Users(string? search, string? role, bool? active, int page = 1, int pageSize = 20)
        {
            var result = await _admin.GetUsersAsync(search, role, active, page, pageSize)
                         ?? new PagedResult<AdminUserListItemDto>(0, page, pageSize, new());

            ViewBag.Search = search;
            ViewBag.Role = role;
            ViewBag.Active = active;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(int id, bool isActive, string? returnUrl = null)
        {
            await _admin.UpdateUserStatusAsync(id, isActive);
            return Redirect(returnUrl ?? Url.Action(nameof(Users))!);
        }

        // Pending PetSitters
        [HttpGet]
        public async Task<IActionResult> PendingPetSitters(int page = 1, int pageSize = 20)
        {
            var result = await _admin.GetPendingPetSittersAsync(page, pageSize)
                         ?? new PagedResult<PendingPetSitterItemVm>(0, page, pageSize, new());

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePetSitter(int id, bool isApproved, string? returnUrl = null)
        {
            await _admin.ApprovePetSitterAsync(id, isApproved);
            return Redirect(returnUrl ?? Url.Action(nameof(PendingPetSitters))!);
        }

        //  Announcements
        [HttpGet]
        public async Task<IActionResult> Announcements(bool? active, int page = 1, int pageSize = 20)
        {
            var result = await _admin.GetAnnouncementsAsync(active, page, pageSize)
                         ?? new PagedResult<AnnouncementAdminItemVm>(0, page, pageSize, new());

            ViewBag.Active = active;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAnnouncementActive(int id, bool isActive, string? returnUrl = null)
        {
            await _admin.SetAnnouncementActiveAsync(id, isActive);
            return Redirect(returnUrl ?? Url.Action(nameof(Announcements))!);
        }

        //  Species 
        [HttpGet]
        public async Task<IActionResult> Species()
        {
            var items = await _crud.GetSpeciesAsync();
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSpecies(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                await _crud.CreateSpeciesAsync(name);

            return RedirectToAction(nameof(Species));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSpecies(int id, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                await _crud.UpdateSpeciesAsync(id, name);

            return RedirectToAction(nameof(Species));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSpecies(int id)
        {
            await _crud.DeleteSpeciesAsync(id);
            return RedirectToAction(nameof(Species));
        }

        // Breeds 
        [HttpGet]
        public async Task<IActionResult> Breeds(int speciesId)
        {
            var species = await _crud.GetSpeciesAsync();
            var breeds = speciesId > 0 ? await _crud.GetBreedsAsync(speciesId) : new List<BreedItemDto>();

            ViewBag.Species = species;
            ViewBag.SpeciesId = speciesId;

            return View(breeds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBreed(int speciesId, string name)
        {
            if (speciesId > 0 && !string.IsNullOrWhiteSpace(name))
                await _crud.CreateBreedAsync(speciesId, name);

            return RedirectToAction(nameof(Breeds), new { speciesId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBreed(int id, int speciesId, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                await _crud.UpdateBreedAsync(id, name);

            return RedirectToAction(nameof(Breeds), new { speciesId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBreed(int id, int speciesId)
        {
            await _crud.DeleteBreedAsync(id);
            return RedirectToAction(nameof(Breeds), new { speciesId });
        }

        // Skills 
        [HttpGet]
        public async Task<IActionResult> Skills()
        {
            var items = await _crud.GetSkillsAsync();
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSkill(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                await _crud.CreateSkillAsync(name);

            return RedirectToAction(nameof(Skills));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSkill(int id, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                await _crud.UpdateSkillAsync(id, name);

            return RedirectToAction(nameof(Skills));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            await _crud.DeleteSkillAsync(id);
            return RedirectToAction(nameof(Skills));
        }
    }

}

