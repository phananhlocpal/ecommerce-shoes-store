using backend.Libraries;
using backend.Models;

namespace backend.Services
{
    public class VNPayService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<VNPayService> _logger;

        public VNPayService(IConfiguration config, ILogger<VNPayService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            if (model == null)
            {
                _logger.LogError("Invalid payment request model.");
                return null;
            }

            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:VnPayVersion"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:VnPayCommand"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:VnPayTmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); // Amount in VND

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:VnPayCurrency"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:VnPayLocale"]);

            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toán cho đơn hàng: {model.OrderId}");
            vnpay.AddRequestData("vnp_OrderType", "other"); // Default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:VnPayPaymentBackReturnUrl"]);

            vnpay.AddRequestData("vnp_TxnRef", tick); // Unique transaction reference
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:VnPayUrl"], _config["VnPay:VnPayHashSecret"]);

            if (string.IsNullOrEmpty(paymentUrl))
            {
                _logger.LogError("Failed to create payment URL.");
            }

            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection queryCollection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in queryCollection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnpOrderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnpTransactionId = vnpay.GetResponseData("vnp_TransactionNo");
            var vnpSecureHash = queryCollection["vnp_SecureHash"];
            var vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnpOrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnpSecureHash, _config["VnPay:VnPayHashSecret"]);
            if (!checkSignature)
            {
                _logger.LogWarning("Invalid signature.");
                return new VnPaymentResponseModel
                {
                    Success = false,
                };
            }

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnpOrderInfo,
                OrderId = vnpOrderId,
                TransactionId = vnpTransactionId,
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode
            };
        }
    }
}
