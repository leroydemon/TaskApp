using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    // Represents a role entity for ASP.NET Core Identity with a GUID as the primary key
    public class Role : IdentityRole<Guid>
    {
    }
}
