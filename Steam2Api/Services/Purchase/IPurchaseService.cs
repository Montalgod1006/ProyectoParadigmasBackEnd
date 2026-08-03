using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Invoice;
using Steam2Api.Dtos.Purchase;

namespace Steam2Api.Services.Purchase
{
    public interface IPurchaseService
    {
        Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(PurchaseCreateDto dto);
        Task<ResponseDto<CreateOrderResponseDto>> CreatePayPalOrderAsync(PurchaseCreateDto dto);
        Task<ResponseDto<InvoiceDto>> CapturePayPalOrderAsync(CaptureOrderDto dto);
    }
}
