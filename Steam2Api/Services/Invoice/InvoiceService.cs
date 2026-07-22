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
        public async Task<ResponseDto<InvoiceActionResponseDto>> CreateAsync(InvoiceCreateDto dto)
        {
            var userExists = await _context.Users.AnyAsync(x => x.Id == dto.UserId);
            if (!userExists)
            {
                return new ResponseDto<InvoiceActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "El usuario no existe."
                };
            }

            var gameIds = dto.Details.Select(x => x.GameId).Distinct().ToList();
            var games = await _context.Games.Where(x => gameIds.Contains(x.Id)).ToListAsync();

            if (games.Count != gameIds.Count)
            {
                return new ResponseDto<InvoiceActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Uno o más juegos no existen."
                };
            }


            var invoice = new InvoiceEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = dto.UserId,
                InvoiceDate = DateTime.UtcNow,
                Total = 0
            };

            var invoiceDetails = new List<InvoiceDetailEntity>();
            decimal total = 0;

            foreach (var detailDto in dto.Details)
            {
                var game = games.First(x => x.Id == detailDto.GameId);

                invoiceDetails.Add(new InvoiceDetailEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    InvoiceId = invoice.Id,
                    GameId = game.Id,
                   
                    UnitPrice = game.Price,
                });

                // total +=
            }

            invoice.Total = total;
            invoice.InvoiceDetails = invoiceDetails;

            await _context.Invoices.AddAsync(invoice);
            _context.InvoiceDetails.AddRange(invoiceDetails);
            _context.Games.UpdateRange(games);
            await _context.SaveChangesAsync();

            return new ResponseDto<InvoiceActionResponseDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = HttpMessageResponse.REGISTER_CREATED,
                Data = new InvoiceActionResponseDto { Id = invoice.Id }
            };
        }

    }
}
