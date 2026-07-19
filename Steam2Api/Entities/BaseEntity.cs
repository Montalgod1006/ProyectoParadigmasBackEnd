using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steam2Api.Entities
{
    public class BaseEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("created_by_id")]
        public string CreatedById { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("update_created_by_id")]
        public string UpdateCreatedById { get; set; }

        [Column("update_created_date")]
        public DateTime UpdateCreatedDate { get; set; }
    }
}