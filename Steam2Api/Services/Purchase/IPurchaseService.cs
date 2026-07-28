using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steam2Api.Dtos.Common;
using Steam2Api.Dtos.Game;
using Steam2Api.Dtos.Invoice;
using Steam2Api.Dtos.Purchase;

namespace Steam2Api.Services.Purchase
{
    public interface IPurchaseService
    {
        Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(PurchaseCreateDto dto);
    }
}
