using Steam2Api.Entities;

namespace Steam2Api.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; } 

        public string UserName { get; set; }
        public List<InvoiceEntity> Invoices { get; set; }
    }
}
