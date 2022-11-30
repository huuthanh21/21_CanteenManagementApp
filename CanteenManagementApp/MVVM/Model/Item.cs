using CanteenManagementApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public class Item: ObservableObject, ICloneable 
    {

        [Key]
        [MaxLength(10)]
        public int Id { get; set; }

        [Required]
        public int Type { get; set; } // 0: Món ăn, 1: Hàng tồn

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public int Amount { get; set; }

        [NotMapped] // Không thêm cột này vào CSDL
        public string ImagePath { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
