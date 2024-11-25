using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication;

namespace SalesDashBoardApplicationProxyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<InventoriesController> _logger;

        public InventoriesController(SalesDashBoardClient salesDashBoardClient, ILogger<InventoriesController> logger)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetInventoryData()
        {
            try
            {
                _logger.LogInformation("Getting inventories data");
                var inventories = await _salesDashBoardClient.InventoriesAllAsync();
                return Ok(inventories);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching inventories data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStockInvetoryData()
        {
            try
            {
                _logger.LogInformation("Getting low stock data");
                var inventories = await _salesDashBoardClient.LowStockAsync();
                return Ok(inventories);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while fetching low stock inventory data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpPost("reorder/{productId}")]
        public async Task<IActionResult> ReorderStock(int productId)
        {
            try
            {
                _logger.LogInformation("Reordering the low stock product");
                await _salesDashBoardClient.ReorderAsync(productId);
                return Ok();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while reordering the product");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        //[Authorize]
        [HttpGet("{productId}")]
        public async Task<IActionResult> InvnetoryByProductId(int productId)
        {
            try
            {
                _logger.LogInformation("Inventory of a product id");
                var invnetory = await _salesDashBoardClient.InventoriesAsync(productId);
                return Ok(invnetory);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while getting the inventory by product id");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }
        
    }
}
