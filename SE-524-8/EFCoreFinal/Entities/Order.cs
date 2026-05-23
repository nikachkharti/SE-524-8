using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreFinal.Entities
{
    public class Order
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime LastLoginDate { get; set; }

        public decimal OrderAmount { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Status { get; set; }

        public decimal Discount { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
