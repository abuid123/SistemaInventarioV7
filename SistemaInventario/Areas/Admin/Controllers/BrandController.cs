using InventorySystem.DataAccess.Repository;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Brand brand = new Brand();

            if (id == null)
            {
                // this is for create a new warehouse
                brand.Active = true;
                return View(brand);
            }
            // this is for edit
            brand = await _unitOfWork.Brand.GetAsync(id.GetValueOrDefault());
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.Id == 0)
                {
                    await _unitOfWork.Brand.AddAsync(brand);
                    TempData[DS.Success] = "Brand created successfully";
                }
                else
                {
                    _unitOfWork.Brand.Update(brand);
                    TempData[DS.Success] = "Brand updated successfully";
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error at save Brand";
            return View(brand);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Brand.GetAllAsync();
            return Json(new { data = allObj });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var brandFromDB = await _unitOfWork.Brand.GetAsync(id);
            if (brandFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Brand.Remove(brandFromDB);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "Delete successful." });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitOfWork.Brand.GetAllAsync();

            if (id == 0)
            {
                value = list.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            }
            else
            {
                value = list.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.Id != id);
            }

            if(value)
            {
                return Json(new { data = true });
            }

            return Json(new { data = false });
        }
        #endregion
    }
}
