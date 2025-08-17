using Shared.Persistence.Abstracts;

namespace AuthService.Domain.Entities
{
    public class OperationClaim : BaseEntity<Guid>
    {
        public string Role { get; set; }
        public ICollection<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
