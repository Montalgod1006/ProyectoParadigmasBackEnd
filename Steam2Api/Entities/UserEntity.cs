using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steam2Api.Entities
{
    [Table("usuarios")]
    public class UserEntity : BaseEntity
    {
        [Required()]
        [Column("user_name")]
        public string UserName { get; set; }

        [Required()]
        [Column("password")]
        public string Password { get; set; }

        public List<InvoiceEntity> Invoices { get; set; } = new();
        public List<GameEntity> Games { get; set; } = new();
        //Todo: Que salgan los juegos que ha comprado. 
    }
}
