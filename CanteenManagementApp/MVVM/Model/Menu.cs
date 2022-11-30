using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CanteenManagementApp.MVVM.Model
{
    /* Principal Entity */
    [Table("Menu")]
    [Keyless]
    public class Menu
    {   
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        public Item Item { get; set; }

    }
}
