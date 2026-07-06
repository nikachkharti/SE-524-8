using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company524.Domain.Entities
{
    public class ProductImage
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [MaxLength(200)]
        public string ImagePublicId { get; set; }
        public bool IsPrimary { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
