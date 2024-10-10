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
        private readonly WhiteLagoonDbContext _context;

        public VillaNumberController(WhiteLagoonDbContext context)
        {
            _context=context;
        }

        public async Task<IActionResult> Index()
        {
            var villaNumbers = await _context.VillaNumbers.Include(VN => VN.Villa).ToListAsync();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel() 
            {
                VillaList = _context.Villas.ToList().Select(VN => new SelectListItem
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
            bool roomNumberExists = _context.VillaNumbers.Any(VN => VN.Villa_Number == villaNumberViewModel.VillaNumber.Villa_Number);
            
            
            if (ModelState.IsValid && !roomNumberExists)
            {
                await _context.VillaNumbers.AddAsync(villaNumberViewModel.VillaNumber);
                TempData["success"] = "The villa number has been created successfully";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (roomNumberExists)
            {
                TempData["error"] = "Room Number is already exist";
            }

            TempData["error"] = "The villa number couldn't created";
            
            villaNumberViewModel.VillaList = _context.Villas.ToList().Select(VN => new SelectListItem
            {
                Text =VN.Name,
                Value = VN.Id.ToString(),
            });
            return View(villaNumberViewModel);
        }

        public IActionResult Update(int villaNo)
        {
            VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel()
            {
                VillaList = _context.Villas.ToList().Select(VN => new SelectListItem
                {
                    Text =VN.Name,
                    Value = VN.Id.ToString(),
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(V => V.Villa_Number == villaNo)
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
                _context.VillaNumbers.Update(villaNumberViewModel.VillaNumber);
                await _context.SaveChangesAsync();
                TempData["success"] = "The villa number has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number couldn't updated";
            villaNumberViewModel.VillaList = _context.Villas.ToList().Select(VN => new SelectListItem
            {
                Text =VN.Name,
                Value = VN.Id.ToString(),
            });
            return View(villaNumberViewModel);
        }

        public IActionResult Delete(int villaNo)
        {

            VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel()
            {
                VillaList = _context.Villas.ToList().Select(VN => new SelectListItem
                {
                    Text =VN.Name,
                    Value = VN.Id.ToString(),
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(V => V.Villa_Number == villaNo)
            };
            if (villaNumberViewModel.VillaNumber != null)
                return View(villaNumberViewModel);
            else
                return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VillaNumberViewModel villaNumberViewModel)
        {
            var villaNumberFormDb = _context.VillaNumbers.FirstOrDefault(VN => VN.Villa_Number == villaNumberViewModel.VillaNumber.Villa_Number);
            if (villaNumberFormDb is not null)
            {
                _context.VillaNumbers.Remove(villaNumberFormDb);
                await _context.SaveChangesAsync();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number couldn't deleted";
            return View(villaNumberViewModel);
        }
    }
}
