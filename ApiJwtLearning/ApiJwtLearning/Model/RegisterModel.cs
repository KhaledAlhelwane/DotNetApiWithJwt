using System.ComponentModel.DataAnnotations;

namespace ApiJwtLearning.Model
{
    public class RegisterModel
    {
        [Required,MaxLength(100)]
        public string firstName { get; set; }

        [Required, MaxLength(100)]
        public string secoundName { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }

    }
}
