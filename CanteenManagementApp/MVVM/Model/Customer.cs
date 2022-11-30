using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CanteenManagementApp.MVVM.Model
{
    /* Principal Entity */
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [MaxLength(10)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string CustomerType { get; set; }

        [Required]
        public float Balance { get; set; }

    }
}
