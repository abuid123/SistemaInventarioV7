using InventorySystem.DataAccess.Repository;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new Category();

            if (id == null)
            {
                // this is for create a new warehouse
                category.Active = true;
                return View(category);
            }
            // this is for edit
            category = await _unitOfWork.Category.GetAsync(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    await _unitOfWork.Category.AddAsync(category);
                    TempData[DS.Success] = "Category created successfully";
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                    TempData[DS.Success] = "Category updated successfully";
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error at save Category";
            return View(category);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Category.GetAllAsync();
            return Json(new { data = allObj });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryFromDB = await _unitOfWork.Category.GetAsync(id);
            if (categoryFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Category.Remove(categoryFromDB);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "Delete successful." });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitOfWork.Category.GetAllAsync();

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
