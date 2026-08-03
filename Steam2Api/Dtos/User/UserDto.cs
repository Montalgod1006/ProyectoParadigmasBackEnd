using Steam2Api.Dtos.Game;
using Steam2Api.Dtos.Invoice;

namespace Steam2Api.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public List<InvoiceDto> Invoices { get; set; } = new();
        public List<GameDto> Games { get; set; } = new();
    }
}
