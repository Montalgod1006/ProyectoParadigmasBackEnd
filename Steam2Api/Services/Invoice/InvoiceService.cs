using Microsoft.EntityFrameworkCore;
using Steam2Api.Constants;
using Steam2Api.Data;
using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Invoice;
using Steam2Api.Entities;
using Steam2Api.Mappers;

namespace Steam2Api.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<List<InvoiceDto>>> GetAllAsync()
        {
            var invoices = await _context.Invoices
                .Include(x => x.InvoiceDetails)
                .OrderByDescending(x => x.InvoiceDate)
                .ToListAsync();

            return new ResponseDto<List<InvoiceDto>>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTERS_FOUND,
                Data = InvoiceMapper.ListEntityToListDto(invoices)
            };
        }

        public async Task<ResponseDto<InvoiceDto>> GetOneByIdAsync(string id)
        {
            var invoice = await _context.Invoices
                .Include(x => x.InvoiceDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (invoice is null)
            {
                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            return new ResponseDto<InvoiceDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = HttpMessageResponse.REGISTER_FOUND,
                Data = InvoiceMapper.EntityToDto(invoice)
            };
        }
    //TODO: Hacer que la factura se cree automáticamente cuando se haga una compra.
       
}

}
