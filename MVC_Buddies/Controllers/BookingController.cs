using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Models;
using System.Reflection;
using System.Security.Claims;

namespace MVC_Buddies.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _http;

        public BookingController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("API_Buddies");
        }


        [HttpGet]
        public async Task<IActionResult> Create(int announcementId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
     "Login",
     "Auth",
     new { returnUrl = Url.Action("Create", "Booking") }
 );

            }

            var vm = new BookingViewModel
            {
                AnnouncementId = announcementId,
                Pets = new List<PetOptionDto>(),
                Skills = new List<SkillOptionDto>(),
                Services = new List<ServiceOptionDto>()
            };



            // 1️⃣ Buscar pets do utilizador 
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var pets = await _http.GetFromJsonAsync<List<PetOptionDto>>($"petowner/{userId}/pets")
              ?? new List<PetOptionDto>();
         
            vm.Pets = pets ?? new List<PetOptionDto>();

     

            // 2️⃣ Buscar anúncio para saber qual é o petsitter
            var announcement = await _http.GetFromJsonAsync<AnnouncementDetailsDto>($"announcements/{announcementId}");
            if (announcement == null)
                return NotFound();

            // 3️⃣ Buscar skills do petsitter
            var skills = await _http.GetFromJsonAsync<List<SkillOptionDto>>($"petsitters/{announcement.IdPetsitter}/skills");
            vm.Skills = skills ?? new List<SkillOptionDto>();

            List<ServiceOptionDto> services;
            try
            {
                services = await _http.GetFromJsonAsync<List<ServiceOptionDto>>($"announcements/{announcementId}/services")
                           ?? new List<ServiceOptionDto>();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Se não existirem services, devolve lista vazia
                services = new List<ServiceOptionDto>();
            }
            vm.Services = services;


            return View(vm);
        }


        [HttpPost]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> Create(BookingViewModel vm)
        {
            var petOwnerId = await _http.GetFromJsonAsync<int>("accounts/me/petownerid");

            var dto = new CreateBookingDto
            {
                PetOwnerId = petOwnerId,
                AnnouncementId = vm.AnnouncementId,
                PetId = vm.PetId,
                Status = "Pending",
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                TotalPrice = vm.TotalPrice,
                Commission = vm.Commission
            };

            var response = await _http.PostAsJsonAsync("bookings", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", error);

                // 🔥 RECARREGAR LISTAS
                await LoadBookingData(vm);

                return View(vm);
            }

            return RedirectToAction("MyBookings");
        }




        private async Task<bool> IsPetSitter(int userId)
        {
            var roles = await _http.GetFromJsonAsync<List<UserRoleDto>>($"users/{userId}/roles");
            return roles.Any(r => r.RoleId == 1); // 1 = PetSitter
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            // O JWT já está no HttpClient via handler/cookies
            var bookings = await _http
                .GetFromJsonAsync<List<BookingListDto>>("bookings/my")
                ?? new List<BookingListDto>();

            // Descobrir se o user é PetSitter ou PetOwner
            bool isPetSitter = User.IsInRole("PetSitter");

            ViewBag.IsPetSitter = isPetSitter;

            return View(bookings);
        }

        [HttpPost]
        [Authorize(Roles = "PetSitter")]
        public async Task<IActionResult> Accept(int id)
        {
            await _http.PutAsync($"bookings/{id}/accept", null);
            return RedirectToAction("MyBookings");
        }

        [HttpPost]
        [Authorize(Roles = "PetSitter")]
        public async Task<IActionResult> Reject(int id)
        {
            await _http.PutAsync($"bookings/{id}/reject", null);
            return RedirectToAction("MyBookings");
        }

        private async Task LoadBookingData(BookingViewModel vm)
        {
            // 1️⃣ Pets do PetOwner
            var petOwnerId = await _http.GetFromJsonAsync<int>("accounts/me/petownerid");


            var pets = await _http.GetFromJsonAsync<List<PetOptionDto>>("accounts/me/pets")
            ?? new List<PetOptionDto>();
            vm.Pets = pets;

            // 2️⃣ Buscar anúncio
            var announcement = await _http.GetFromJsonAsync<AnnouncementDetailsDto>(
                $"announcements/{vm.AnnouncementId}");

            if (announcement != null)
            {
                // 3️⃣ Skills
                var skills = await _http.GetFromJsonAsync<List<SkillOptionDto>>(
                    $"petsitters/{announcement.IdPetsitter}/skills")
                    ?? new List<SkillOptionDto>();

                vm.Skills = skills;

                // 4️⃣ Services
                var services = await _http.GetFromJsonAsync<List<ServiceOptionDto>>(
                    $"announcements/{vm.AnnouncementId}/services")
                    ?? new List<ServiceOptionDto>();

                vm.Services = services;
            }
        }


    }

}
