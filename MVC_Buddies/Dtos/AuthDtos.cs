using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{
    public enum RegisterType
    {
        PetOwner = 0,
        PetSitter = 1
    }

    public class RegisterDto
    {
        [Required]
        public RegisterType Type { get; set; } = RegisterType.PetOwner;

        [Required(ErrorMessage = "Nome completo é obrigatório.")]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username é obrigatório.")]
        [StringLength(40)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatória.")]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação de password é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "As passwords não coincidem.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }



    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }

    public class HashPasswordDto
    {
        public string Password { get; set; } = string.Empty;
    }

}
