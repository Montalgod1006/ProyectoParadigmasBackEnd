using System.ComponentModel.DataAnnotations;

namespace Steam2Api.Dtos.Purchase
{
    public class PurchaseCreateDto
    {
        [Required(ErrorMessage = "El id del usuario es obligatorio")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Debe enviar al menos un juego")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos un juego")]
        public List<string> GameIds { get; set; } = new();
    }
}