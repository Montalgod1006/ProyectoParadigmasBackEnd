using System.ComponentModel.DataAnnotations;
using Steam2Api.Entities;

namespace Steam2Api.Dtos.Invoice
{
    public class InvoiceDetailCreateDto : BaseEntity
    {
        [Required(ErrorMessage = "El id del juego es obligatorio")]
        public string GameId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }
}