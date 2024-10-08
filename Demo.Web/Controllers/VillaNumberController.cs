using Demo.Domain.Entities;
using Demo.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var villaNumbers = await _context.VillaNumbers.ToListAsync();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumber villaNumber)
        {
            if (ModelState.IsValid)
            {
                await _context.VillaNumbers.AddAsync(villaNumber);
                TempData["success"] = "The villa number has been created successfully";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number couldn't created";
            return View(villaNumber);
        }

        public IActionResult Update(int villaNo)
        {
            var villaNumber = _context.VillaNumbers.FirstOrDefault(V => V.Villa_Number == villaNo);
            if (villaNumber != null)
                return View(villaNumber);
            else
                return RedirectToAction("Error" , "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Update(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _context.Villas.Update(villa);
                await _context.SaveChangesAsync();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa couldn't updated";
            return View(villa);
        }

        public IActionResult Delete(int villaNo)
        {
            var villa = _context.Villas.FirstOrDefault(V => V.Id == villaNo);
            if(villa != null)
                return View(villa);
            return RedirectToAction("Error" , "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Villa villa)
        {
            var villaFormDb = _context.Villas.FirstOrDefault(V => V.Id == villa.Id);
            if (villaFormDb is not null)
            {
                _context.Villas.Remove(villaFormDb);
                await _context.SaveChangesAsync();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa couldn't deleted";
            return View(villa);
        }
    }
}
