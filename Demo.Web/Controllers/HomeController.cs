using Demo.Application.Common.Interfaces;
using Demo.Web.Models;
using Demo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo.Web.Controllers
{
	public class HomeController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
		{
			HomeViewModel model = new HomeViewModel()
			{
				VillaList = await _unitOfWork.Villa.GetAll(),
				Nights = 1,
				CheckInDate = DateOnly.FromDateTime(DateTime.Now),
			};
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
