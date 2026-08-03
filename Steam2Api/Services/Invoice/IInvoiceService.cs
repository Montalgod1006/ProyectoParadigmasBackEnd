using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Invoice;

namespace Steam2Api.Services.Invoice
{
    public interface IInvoiceService
    {
        Task<ResponseDto<List<InvoiceDto>>> GetAllAsync();
        Task<ResponseDto<InvoiceDto>> GetOneByIdAsync(string id);
        Task<ResponseDto<InvoiceDto>> GetOneByIdUserAsync(string id);
    }
}
