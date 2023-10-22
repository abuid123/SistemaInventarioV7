using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Name can't be more than 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description required")]
        [MaxLength(100, ErrorMessage = "Description can't be more than 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Active is required")]
        public bool Active { get; set; }
    }
}
