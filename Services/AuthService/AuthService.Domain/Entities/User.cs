using Shared.Persistence.Abstracts;

namespace AuthService.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public ICollection<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
