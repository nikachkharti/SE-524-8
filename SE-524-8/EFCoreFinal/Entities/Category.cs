using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreFinal.Entities
{
    public class Category
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
