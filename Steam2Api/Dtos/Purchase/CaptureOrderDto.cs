using System.ComponentModel.DataAnnotations;

namespace Steam2Api.Dtos.Purchase
{
    public class CaptureOrderDto
    {
        [Required(ErrorMessage = "El id de la orden de PayPal es obligatorio")]
        public string OrderId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El id del usuario es obligatorio")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La lista de juegos es obligatoria")]
        public List<string> GameIds { get; set; } = new();
    }
}