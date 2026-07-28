using Microsoft.EntityFrameworkCore;
using Steam2Api.Constants;
using Steam2Api.Data;
using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Invoice;
using Steam2Api.Dtos.Purchase;
using Steam2Api.Entities;
using Steam2Api.Mappers;

namespace Steam2Api.Services.Purchase
{
    public class PurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;

        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(PurchaseCreateDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Invoices)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
            {
                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            var gameIds = dto.GameIds;

            if (gameIds == null || gameIds.Count == 0)
            {   
                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "La lista debe contener al menos un juego."
                };
            }


            var normalizedGames = gameIds
                .Select(id => id.Trim())
                .Distinct()
                .ToList();


            var games = await _context.Games
                .Where(game => normalizedGames.Contains(game.Id))
                .ToListAsync();                                                                  
                                                                                                                                

            if (games.Count != normalizedGames.Count)
            {
                var existingGameIds = games
                    .Select(game => game.Id)
                    .ToList();

                var missingGames = normalizedGames
                    .Except(existingGameIds)
                    .ToList();


                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = $"Estos juegos no existen en la base de datos: {string.Join(", ", missingGames)}"
                };
            }


            var total = games.Sum(game => game.Price);


            var invoice = new InvoiceEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                InvoiceDate = DateTime.UtcNow,
                Total = total,
                InvoiceDetails = new List<InvoiceDetailEntity>()
            };


            foreach (var game in games)
            {
                var invoiceDetail = new InvoiceDetailEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    InvoiceId = invoice.Id,
                    GameId = game.Id,
                    UnitPrice = game.Price
                };

                invoice.InvoiceDetails.Add(invoiceDetail);
            }

            user.Invoices.Add(invoice);
            _context.Invoices.Add(invoice);

            await _context.SaveChangesAsync();

            var response = InvoiceMapper.EntityToDto(invoice);
            return new ResponseDto<InvoiceDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = "Compra realizada correctamente.",
                Data = response
            };
        }


    }
}