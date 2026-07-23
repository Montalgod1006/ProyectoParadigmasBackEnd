using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steam2Api.Entities
{
    [Table("facturas")]
    public class InvoiceEntity : BaseEntity
    {
        [Required()]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required()]
        [Column("invoice_date")]
        public DateTime InvoiceDate { get; set; }

        [Required()]
        [Column("total")]
        public decimal Total { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

        public List<InvoiceDetailEntity> InvoiceDetails { get; set; } = new();
    }
}
