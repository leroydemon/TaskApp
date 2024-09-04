using Microsoft.AspNetCore.Identity;

namespace BussinesLogic.EntityDtos
{
    public class UserDto : IdentityUser<Guid>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
