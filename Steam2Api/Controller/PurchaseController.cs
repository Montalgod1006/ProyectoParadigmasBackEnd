using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steam2Api.Dtos.Purchase;
using Steam2Api.Services.Invoice;
using Steam2Api.Services.Purchase;

namespace Steam2Api.Controller
{
    [ApiController]
    [Route("api/purchase")]
    public class PurchaseController : ControllerBase
    {
        private IPurchaseService _purchaseService;

        public PurchaseController (IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateInvoiceAsync (PurchaseCreateDto dto)
        {
            var response = await _purchaseService.CreateInvoiceAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

    }
}