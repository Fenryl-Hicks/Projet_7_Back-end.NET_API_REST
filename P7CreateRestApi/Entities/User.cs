using Microsoft.AspNetCore.Identity;

namespace P7CreateRestApi.Entities
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        
    }
}
