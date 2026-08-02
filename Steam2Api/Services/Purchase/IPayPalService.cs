namespace Steam2Api.Services.Purchase
{
    public class PayPalCaptureResult
    {
        public bool Success { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PayPalOrderId { get; set; } = string.Empty;
    }
 
    public interface IPayPalService
    {
        Task<string> CreateOrderAsync(decimal amount, string currencyCode = "USD");
        Task<PayPalCaptureResult> CaptureOrderAsync(string payPalOrderId);
    }
}