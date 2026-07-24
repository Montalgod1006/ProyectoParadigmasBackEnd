using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steam2Api.Entities
{
     [Table("juegos")]
    public class GameEntity : BaseEntity
    {
        [Required()]
        [Column("name")]
        public string Name { get; set; }

        [Required()]
        [Column("genre")]
        public string Genre { get; set; }

        [Required()]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        public List<InvoiceDetailEntity> InvoiceDetails { get; set; } = new();
    }
}
