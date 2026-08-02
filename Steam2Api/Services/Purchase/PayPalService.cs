using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Steam2Api.Services.Purchase;

namespace Steam2Api.Services.PayPal
{
    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // El token de acceso de PayPal dura 8-9 horas; lo cacheamos en memoria para no pedirlo en cada request
        private static string? _cachedAccessToken;
        private static DateTime _cachedAccessTokenExpiresAt = DateTime.MinValue;
        private static readonly SemaphoreSlim _tokenLock = new(1, 1);

        public PayPalService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var baseUrl = _configuration["PayPal:BaseUrl"] ?? "https://api-m.sandbox.paypal.com";
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            if (_cachedAccessToken != null && DateTime.UtcNow < _cachedAccessTokenExpiresAt)
            {
                return _cachedAccessToken;
            }

            await _tokenLock.WaitAsync();
            try
            {
                // Puede que otro request ya haya refrescado el token mientras esperábamos el lock
                if (_cachedAccessToken != null && DateTime.UtcNow < _cachedAccessTokenExpiresAt)
                {
                    return _cachedAccessToken;
                }

                var clientId = _configuration["PayPal:ClientId"];
                var clientSecret = _configuration["PayPal:ClientSecret"];

                if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                {
                    throw new InvalidOperationException(
                        "Falta configurar PayPal:ClientId y PayPal:ClientSecret (appsettings.Development.json o user-secrets).");
                }

                var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials"
                });

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var tokenResponse = await response.Content.ReadFromJsonAsync<PayPalTokenResponse>();

                if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    throw new InvalidOperationException("PayPal no devolvió un access token válido.");
                }

                _cachedAccessToken = tokenResponse.AccessToken;
                // Restamos 60s de margen de seguridad antes de que expire de verdad
                _cachedAccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60);

                return _cachedAccessToken;
            }
            finally
            {
                _tokenLock.Release();
            }
        }

        public async Task<string> CreateOrderAsync(decimal amount, string currencyCode = "USD")
        {
            var accessToken = await GetAccessTokenAsync();

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = currencyCode,
                            value = amount.ToString("F2", CultureInfo.InvariantCulture)
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders")
            {
                Content = JsonContent.Create(body)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Error creando la orden en PayPal: {responseBody}");
            }

            using var json = JsonDocument.Parse(responseBody);
            var orderId = json.RootElement.GetProperty("id").GetString();

            if (string.IsNullOrEmpty(orderId))
            {
                throw new InvalidOperationException("PayPal no devolvió un id de orden.");
            }

            return orderId;
        }

        public async Task<PayPalCaptureResult> CaptureOrderAsync(string payPalOrderId)
        {
            var accessToken = await GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{payPalOrderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // El body va vacío; PayPal solo necesita el content-type para aceptar la captura
            request.Content = new StringContent(string.Empty);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PayPalCaptureResult
                {
                    Success = false,
                    Status = "FAILED",
                    PayPalOrderId = payPalOrderId
                };
            }

            using var json = JsonDocument.Parse(responseBody);
            var status = json.RootElement.GetProperty("status").GetString() ?? "UNKNOWN";

            return new PayPalCaptureResult
            {
                Success = status == "COMPLETED",
                Status = status,
                PayPalOrderId = payPalOrderId
            };
        }

        private class PayPalTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }
    }
}