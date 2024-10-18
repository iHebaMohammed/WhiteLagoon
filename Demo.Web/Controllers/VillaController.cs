using Demo.Application.Common.Interfaces;
using Demo.Domain.Entities;
using Demo.Infrastructure.Data;
using Demo.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork=unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                if(villa.Image != null)
                {
                    villa.ImageUrl = await DocumentSettings.UploadFile(villa.Image , "VillaImages");
                }
                else
                {
                    villa.ImageUrl = "\\images\\placeholder.png";
                }
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
                if (villa.Image != null)
                {
                    if (!string.IsNullOrEmpty(villa.ImageUrl))
                    {
                        DocumentSettings.DeleteFile(villa.ImageUrl);
                    }
                    villa.ImageUrl = await DocumentSettings.UploadFile(villa.Image, "VillaImages");
                }
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
                DocumentSettings.DeleteFile(villaFormDb.ImageUrl);

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
