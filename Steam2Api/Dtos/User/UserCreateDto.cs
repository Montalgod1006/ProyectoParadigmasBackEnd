using System.ComponentModel.DataAnnotations;

namespace Steam2Api.Dtos.User
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 255 caracteres")]
        public string Password { get; set; }
    }
}