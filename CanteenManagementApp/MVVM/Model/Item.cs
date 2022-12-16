using CanteenManagementApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CanteenManagementApp.MVVM.Model
{
    /* Principal Entity */
    /* Item one-to-many Receipt_Item */

    [Table("Item")]
    public class Item : ObservableObject, ICloneable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        // Navigation Property
        public List<Receipt_Item> Receipt_Items { get; set; }

        [NotMapped] // Không thêm cột này vào CSDL
        public string ImagePath { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    internal class ItemComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null)
                return false;

            //Check whether the items' properties are equal.
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] Item item)
        {
            //Check whether the object is null
            if (item is null) return 0;

            //Get hash code for the Name field if it is not null.
            int hashProductName = item.Name == null ? 0 : item.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = item.Id.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }
    }
}