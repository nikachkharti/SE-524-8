using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieDB.Entities
{
    public class FilmBenchmark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilmId { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
