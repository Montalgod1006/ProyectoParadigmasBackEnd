using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steam2Api.Entities
{
    [Table("detalle_facturas")]
    public class InvoiceDetailEntity : BaseEntity
    {
        [Required()]
        [Column("invoice_id")]
        public string InvoiceId { get; set; }

        [Required()]
        [Column("game_id")]
        public string GameId { get; set; }

        [Required()]
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Required()]
        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public InvoiceEntity Invoice { get; set; }

        [ForeignKey(nameof(GameId))]
        public GameEntity Game { get; set; }
    }
}
