using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication;

namespace SalesDashBoardApplicationProxyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalsController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<PayPalsController> _logger;

        public PayPalsController(SalesDashBoardClient salesDashBoardClient, ILogger<PayPalsController> logger)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
        }



        [HttpPost("create-order")]
        public async Task<IActionResult> InitialiseOrder([FromBody] double amount)
        {
            try
            {
                var orderId = await _salesDashBoardClient.CreateOrderAsync(amount);
                return Ok(new { OrderId = orderId });
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while initialsing a new order");
                return StatusCode(500, new { error = "Could not process due to some error" });  
            }
        }



        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var message = await _salesDashBoardClient.CaptureOrderAsync(orderId);
                return Ok(new { Message = message });
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while capturing the order by orderId");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }
    }
}
