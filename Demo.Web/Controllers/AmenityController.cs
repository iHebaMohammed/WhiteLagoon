using Demo.Application.Common.Interfaces;
using Demo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var amenities = await _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        public async Task<IActionResult> Create()
        {
            AmenityViewModel AmenityViewModel = new AmenityViewModel()
            {
                VillaList = (await _unitOfWork.Villa.GetAll()).Select(a => new SelectListItem
                {
                    Text =a.Name,
                    Value = a.Id.ToString(),
                })
            };
            return View(AmenityViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AmenityViewModel AmenityViewModel)
        {


            if (ModelState.IsValid)
            {
                await _unitOfWork.Amenity.Add(AmenityViewModel.Amenity);
                TempData["success"] = "The amenity has been created successfully";
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The amenity couldn't created";

            AmenityViewModel.VillaList = (await _unitOfWork.Villa.GetAll()).Select(a => new SelectListItem
            {
                Text =a.Name,
                Value = a.Id.ToString(),
            });
            return View(AmenityViewModel);
        }

        public async Task<IActionResult> Update(int amenityId)
        {
            AmenityViewModel amenityViewModel = new AmenityViewModel()
            {
                VillaList = (await _unitOfWork.Villa.GetAll()).Select(a => new SelectListItem
                {
                    Text =a.Name,
                    Value = a.Id.ToString(),
                }),
                Amenity = await _unitOfWork.Amenity.Get(a => a.Id == amenityId)
            };
            if (amenityViewModel.Amenity != null)
                return View(amenityViewModel);
            else
                return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Update(AmenityViewModel AmenityViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(AmenityViewModel.Amenity);
                await _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity couldn't updated";
            AmenityViewModel.VillaList = (await _unitOfWork.Villa.GetAll()).Select(a => new SelectListItem
            {
                Text =a.Name,
                Value = a.Id.ToString(),
            });
            return View(AmenityViewModel);
        }

        public async Task<IActionResult> Delete(int amenityId)
        {

            AmenityViewModel AmenityViewModel = new AmenityViewModel()
            {
                VillaList = (await _unitOfWork.Villa.GetAll()).Select(a => new SelectListItem
                {
                    Text =a.Name,
                    Value = a.Id.ToString(),
                }),
                Amenity = await _unitOfWork.Amenity.Get(a => a.Id == amenityId)
            };
            if (AmenityViewModel.Amenity != null)
                return View(AmenityViewModel);
            else
                return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AmenityViewModel AmenityViewModel)
        {
            var villaNumberFormDb = await _unitOfWork.Amenity.Get(a => a.Id == AmenityViewModel.Amenity.Id);
            if (villaNumberFormDb is not null)
            {
                _unitOfWork.Amenity.Remove(villaNumberFormDb);
                await _unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity couldn't deleted";
            return View(AmenityViewModel);
        }
    }
}
