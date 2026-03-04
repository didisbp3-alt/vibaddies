using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Services;
using MVC_Buddies.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace MVC_MINHA.Controllers
{
    
    public class PetController : Controller
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        public async Task<IActionResult> Index()
        {
            var pets = await _petService.GetMyPetsAsync();
            return View(pets);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
                return NotFound();

            return View(pet);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Breeds = await _petService.GetBreedsAsync();
            return View(new CreatePetDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePetDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Breeds = await _petService.GetBreedsAsync();
                return View(model);
            }

            int id = await _petService.CreatePetAsync(model);
            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
                return NotFound();

            var model = new UpdatePetDto
            {
                Name = pet.Name,
                Age = pet.Age,
                BreedId = pet.BreedId,
                HasSpecialNeeds = pet.HasSpecialNeeds,
                SpecialNeedsNotes = pet.SpecialNeedsNotes,
                MedicalNotes = pet.MedicalNotes,
                PhotoUrls = pet.PhotoUrls
            };

            ViewBag.Breeds = await _petService.GetBreedsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePetDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Breeds = await _petService.GetBreedsAsync();
                return View(model);
            }

            await _petService.UpdatePetAsync(id, model);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _petService.DeletePetAsync(id);
            return RedirectToAction("Index");
        }
    }
}
