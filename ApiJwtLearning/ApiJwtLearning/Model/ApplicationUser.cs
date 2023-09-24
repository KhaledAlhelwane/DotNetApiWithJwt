using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiJwtLearning.Model
{
    public class ApplicationUser:IdentityUser
    {
        [Required,MaxLength(30)]
    public string FirstName { get; set; }
        [Required, MaxLength(30)]
    public string SecoundName { get; set; }
}
}
