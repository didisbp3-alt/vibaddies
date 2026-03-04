using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Models;
using MVC_Buddies.Services;
using System.Diagnostics;

namespace MVC_Buddies.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeApiService _home;

        public HomeController(HomeApiService home)
        {
            _home = home;
        }
   
        [HttpGet]
        public async Task<IActionResult> Index(
    string? location,
    DateTime? from,
    DateTime? to,
    int? serviceId)
        {
            MainViewModel model = new MainViewModel();

            bool isSearch =
                !string.IsNullOrWhiteSpace(location)
                || from.HasValue
                || to.HasValue
                || serviceId.HasValue;

            try
            {
                // ?? SEMPRE carregar sitters (com ou sem filtros)
                model = await _home.SearchAsync(location, from, to, serviceId);

                // Só indica se foi pesquisa ou não (para UI)
                model.IsSearch = isSearch;
            }
            catch
            {
                model = new MainViewModel
                {
                    Sitters = new List<SitterCardDto>(),
                    IsSearch = false
                };
            }

            // Serviços sempre
            model.Services = await _home.GetServicesAsync();
            model.SelectedServiceId = serviceId;

            // Opcional: esconder localização quando não é pesquisa
            if (!isSearch)
            {
                model.Location = null;
            }

            return View(model);
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public IActionResult HowItWorks()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Search(
    string? location,
    DateTime? from,
    DateTime? to,
    int? serviceId)
        {


            MainViewModel model = new MainViewModel();

            bool isSearch =
                !string.IsNullOrWhiteSpace(location)
                || from.HasValue
                || to.HasValue
                || serviceId.HasValue;

            if (isSearch)
            {
                model = await _home.SearchAsync(location, from, to, serviceId);
                model.IsSearch = true;
            }
            else
            {
                // Não carregar Sitters — deixar lista vazia
                model.Sitters = new List<SitterCardDto>();
                model.IsSearch = false;
                model.Location = location; // pode ser null
            }

            // Serviços sempre
            model.Services = await _home.GetServicesAsync();
            model.SelectedServiceId = serviceId;

            // Se quiseres, podes ajustar o título para não mostrar cidade quando não há pesquisa
            // Exemplo:
            if (!isSearch)
            {
                model.Location = null; // ou ""
            }

            return View(model);



        }

    }
}
