using InventorySystem.DataAccess.Data;
using InventorySystem.DataAccess.Repository.IRepository;
using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository
{
    public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
    {
        private readonly ApplicationDbContext _db;

        public WarehouseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Warehouse warehouse)
        {
            var objFromDb = _db.Warehouse.FirstOrDefault(s => s.Id == warehouse.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = warehouse.Name;
                objFromDb.Description = warehouse.Description;
                objFromDb.Active = warehouse.Active;
                _db.SaveChanges();
            }
        }
    }
}
