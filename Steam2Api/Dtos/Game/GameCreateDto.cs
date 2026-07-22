using System.ComponentModel.DataAnnotations;

namespace Steam2Api.Dtos.Game
{
    public class GameCreateDto
    {
        [Required(ErrorMessage = "El nombre del juego es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(100, ErrorMessage = "El género no puede exceder 100 caracteres")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public bool State { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "La URL de la imagen no puede exceder 500 caracteres")]
        public string ImageUrl { get; set; }
    }
}