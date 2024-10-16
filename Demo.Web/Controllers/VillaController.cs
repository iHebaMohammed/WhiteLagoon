using Demo.Application.Common.Interfaces;
using Demo.Domain.Entities;
using Demo.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var villas = await _unitOfWork.Villa.GetAll();
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
                await _unitOfWork.Villa.Add(villa);
                TempData["success"] = "The villa has been created successfully";
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa couldn't created";
                return View(villa);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var villa = await _unitOfWork.Villa.Get(V => V.Id == id);
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
                _unitOfWork.Villa.Update(villa);
                await _unitOfWork.Save();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa couldn't updated";
            return View(villa);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var villa = await _unitOfWork.Villa.Get(V => V.Id == id);
            if(villa != null)
                return View(villa);
            return RedirectToAction("Error" , "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Villa villa)
        {
            var villaFormDb = await _unitOfWork.Villa.Get(V => V.Id == villa.Id);
            if (villaFormDb is not null)
            {
                _unitOfWork.Villa.Remove(villaFormDb);
                await _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa couldn't deleted";
            return View(villa);
        }
    }
}
