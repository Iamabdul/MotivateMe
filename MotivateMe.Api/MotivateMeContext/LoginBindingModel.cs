using System.ComponentModel.DataAnnotations;

namespace MotivateMe.Api.MotivateMeContext
{
    public class LoginBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
