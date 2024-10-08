using Demo.Domain.Entities;
using Demo.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly WhiteLagoonDbContext _context;

        public VillaController(WhiteLagoonDbContext context)
        {
            _context=context;
        }

        public async Task<IActionResult> Index()
        {
            var villas = await _context.Villas.ToListAsync();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Villa villa)
        {
            if (ModelState.IsValid)
            {
                await _context.Villas.AddAsync(villa);
                TempData["success"] = "The villa has been created successfully";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa couldn't created";
                return View(villa);
            }
        }

        public IActionResult Update(int id)
        {
            var villa = _context.Villas.FirstOrDefault(V => V.Id == id);
            if (villa != null)
                return View(villa);
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

        public IActionResult Delete(int id)
        {
            var villa = _context.Villas.FirstOrDefault(V => V.Id == id);
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
