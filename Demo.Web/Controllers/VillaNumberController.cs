using Demo.Application.Common.Interfaces;
using Demo.Domain.Entities;
using Demo.Infrastructure.Data;
using Demo.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Demo.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var villaNumbers = await _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public async Task<IActionResult> Create()
        {
            VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel() 
            {
                VillaList = (await _unitOfWork.Villa.GetAll()).Select(VN => new SelectListItem
                {
                    Text =VN.Name,
                    Value = VN.Id.ToString(),
                })
            };
            return View(villaNumberViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumberViewModel villaNumberViewModel)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(VN => VN.Villa_Number == villaNumberViewModel.VillaNumber.Villa_Number);
            
            
            if (ModelState.IsValid && !roomNumberExists)
            {
                await _unitOfWork.VillaNumber.Add(villaNumberViewModel.VillaNumber);
                TempData["success"] = "The villa number has been created successfully";
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            if (roomNumberExists)
            {
                TempData["error"] = "Room Number is already exist";
            }

            TempData["error"] = "The villa number couldn't created";
            
            villaNumberViewModel.VillaList = (await _unitOfWork.Villa.GetAll()).Select(VN => new SelectListItem
            {
                Text =VN.Name,
                Value = VN.Id.ToString(),
            });
            return View(villaNumberViewModel);
        }

        public async Task<IActionResult> Update(int villaNo)
        {
            VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel()
            {
                VillaList = (await _unitOfWork.Villa.GetAll()).Select(VN => new SelectListItem
                {
                    Text =VN.Name,
                    Value = VN.Id.ToString(),
                }),
                VillaNumber = await _unitOfWork.VillaNumber.Get(V => V.Villa_Number == villaNo)
            };
            if (villaNumberViewModel.VillaNumber != null)
                return View(villaNumberViewModel);
            else
                return RedirectToAction("Error" , "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Update(VillaNumberViewModel villaNumberViewModel)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberViewModel.VillaNumber);
                await _unitOfWork.Save();
                TempData["success"] = "The villa number has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number couldn't updated";
            villaNumberViewModel.VillaList = (await _unitOfWork.Villa.GetAll()).Select(VN => new SelectListItem
            {
                Text =VN.Name,
                Value = VN.Id.ToString(),
            });
            return View(villaNumberViewModel);
        }

        public async Task<IActionResult> Delete(int villaNo)
        {

            VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel()
            {
                VillaList = (await _unitOfWork.Villa.GetAll()).Select(VN => new SelectListItem
                {
                    Text =VN.Name,
                    Value = VN.Id.ToString(),
                }),
                VillaNumber = await _unitOfWork.VillaNumber.Get(V => V.Villa_Number == villaNo)
            };
            if (villaNumberViewModel.VillaNumber != null)
                return View(villaNumberViewModel);
            else
                return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VillaNumberViewModel villaNumberViewModel)
        {
            var villaNumberFormDb = await _unitOfWork.VillaNumber.Get(VN => VN.Villa_Number == villaNumberViewModel.VillaNumber.Villa_Number);
            if (villaNumberFormDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(villaNumberFormDb);
                await _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number couldn't deleted";
            return View(villaNumberViewModel);
        }
    }
}
