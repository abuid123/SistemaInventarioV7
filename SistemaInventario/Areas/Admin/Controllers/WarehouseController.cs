using InventorySystem.DataAccess.Repository;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehouseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Warehouse warehouse = new Warehouse();

            if (id == null)
            {
                // this is for create a new warehouse
                warehouse.Active = true;
                return View(warehouse);
            }
            // this is for edit
            warehouse = await _unitOfWork.Warehouse.GetAsync(id.GetValueOrDefault());
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                if (warehouse.Id == 0)
                {
                    await _unitOfWork.Warehouse.AddAsync(warehouse);
                    TempData[DS.Success] = "Warehouse created successfully";
                }
                else
                {
                    _unitOfWork.Warehouse.Update(warehouse);
                    TempData[DS.Success] = "Warehouse updated successfully";
                }
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error at save warehouse";
            return View(warehouse);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Warehouse.GetAllAsync();
            return Json(new { data = allObj });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var warehouseFromDb = await _unitOfWork.Warehouse.GetAsync(id);
            if (warehouseFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Warehouse.Remove(warehouseFromDb);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "Delete successful." });
        }

        [ActionName("ValidateName")]
        public async Task<IActionResult> ValidateName(string name, int id = 0)
        {
            bool value = false;
            var list = await _unitOfWork.Warehouse.GetAllAsync();

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
