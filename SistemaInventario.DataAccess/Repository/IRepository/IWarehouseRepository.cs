using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.DataAccess.Repository.IRepository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        void Update(Warehouse warehouse);
    }
}
