using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company524.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(512)]
        public string Token { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        /// <summary>
        /// როდის გასდის refresh ტოკენს ვადა
        /// </summary>
        public DateTimeOffset ExpiresAt { get; set; }

        /// <summary>
        /// როდის შეიქმენა refresh ტოკენი
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// როდის გაუქმდა refresh ტოკენი
        /// </summary>
        public DateTimeOffset? RevokedAt { get; set; }

        public bool IsExpired => DateTimeOffset.Now >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsExpired && !IsRevoked;
    }
}
