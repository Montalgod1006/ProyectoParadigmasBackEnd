using Microsoft.AspNetCore.Mvc;
using Steam2Api.Dtos.Purchase;
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

        [HttpPost("create-order")]
        public async Task<ActionResult> CreatePayPalOrderAsync(PurchaseCreateDto dto)
        {
            var response = await _purchaseService.CreatePayPalOrderAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("capture-order")]
        public async Task<ActionResult> CapturePayPalOrderAsync(CaptureOrderDto dto)
        {
            var response = await _purchaseService.CapturePayPalOrderAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

    }
}