using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CanteenManagementApp.MVVM.Model
{
    /* Dependent Entity */
    /* Receipt_Item many-to-one Item */
    /* Receipt_Item many-to-one Receipt */
    [Table("Receipt_Item")]
    [Owned]
    public class Receipt_Item
    {
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required]
        public int Amount { get; set; }

    }

    
}
