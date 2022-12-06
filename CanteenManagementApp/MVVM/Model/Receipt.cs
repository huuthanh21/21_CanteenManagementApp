using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CanteenManagementApp.MVVM.Model
{
    /* Dependent Entity */
    /* Receipt many-to-one Customer */

    [Table("Receipt")]
    public class Receipt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Navigation Property
        public List<Receipt_Item> Receipt_Items { get; set; }

        // Foreign Key
        [Required]
        public string CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime DateTime { get; set; }

        [Required, MaxLength(20)]
        public string PaymentMethod { get; set; }

        [Required]
        public float Total { get; set; }
    }
}