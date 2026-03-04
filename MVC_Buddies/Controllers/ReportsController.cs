using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Models;
using MVC_Buddies.Services;
using System.Security.Claims;


namespace MVC_Buddies.Controllers
{
  
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportsApiService _reportsService;

        public ReportsController(IReportsApiService reportsService)
        {
            _reportsService = reportsService;
        }

        public async Task<IActionResult> Profits(int? year)
        {
            year ??= DateTime.Now.Year;
            var vm = await _reportsService.GetProfitReportAsync(year.Value);
            return View(vm);
        }
    }
}