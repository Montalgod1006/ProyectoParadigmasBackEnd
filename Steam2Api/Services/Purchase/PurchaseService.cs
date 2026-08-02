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
        private readonly IPayPalService _payPalService;

        public PurchaseService(AppDbContext context, IPayPalService payPalService)
        {
            _context = context;
            _payPalService = payPalService;
        }

        public async Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(PurchaseCreateDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Invoices)
                .Include(u => u.Games)
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
            foreach (var game in games)
            {
                if (!user.Games.Any(userGame => userGame.Id == game.Id))
                {
                    user.Games.Add(game);
                }
            }
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

        public async Task<ResponseDto<CreateOrderResponseDto>> CreatePayPalOrderAsync(PurchaseCreateDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Invoices)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user is null)
            {
                return new ResponseDto<CreateOrderResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = HttpMessageResponse.REGISTER_NOT_FOUND
                };
            }

            var gameIds = dto.GameIds;

            if (gameIds == null || gameIds.Count == 0)
            {
                return new ResponseDto<CreateOrderResponseDto>
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

                return new ResponseDto<CreateOrderResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = $"Estos juegos no existen en la base de datos: {string.Join(", ", missingGames)}"
                };
            }

            var total = games.Sum(game => game.Price);

            string payPalOrderId;
            try
            {
                payPalOrderId = await _payPalService.CreateOrderAsync(total);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CreateOrderResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_GATEWAY,
                    Status = false,
                    Message = $"No se pudo crear la orden en PayPal: {ex.Message}"
                };
            }

            return new ResponseDto<CreateOrderResponseDto>
            {
                StatusCode = HttpStatusCode.Ok,
                Status = true,
                Message = "Orden de PayPal creada correctamente.",
                Data = new CreateOrderResponseDto { PayPalOrderId = payPalOrderId }
            };
        }

        public async Task<ResponseDto<InvoiceDto>> CapturePayPalOrderAsync(CaptureOrderDto dto)
        {
            PayPalCaptureResult captureResult;
            try
            {
                captureResult = await _payPalService.CaptureOrderAsync(dto.OrderId);
            }
            catch (Exception ex)
            {
                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.BAD_GATEWAY,
                    Status = false,
                    Message = $"No se pudo capturar el pago en PayPal: {ex.Message}"
                };
            }

            if (!captureResult.Success)
            {
                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = $"El pago no pudo completarse (estado de PayPal: {captureResult.Status})."
                };
            }

            var purchaseDto = new PurchaseCreateDto
            {
                UserId = dto.UserId,
                GameIds = dto.GameIds
            };

            return await CreateInvoiceAsync(purchaseDto);
        }
    }
}
