using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITStepClass.Entities
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        //FOREIGN KEY
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; } //1
        public Department Department { get; set; } //1

        public ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
