using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SalesDashBoardApplication;
using SalesDashBoardApplicationProxyService.Services;

namespace SalesDashBoardApplicationProxyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<RevenuesController> _logger;

        private readonly IHubContext<SalesHub> _hubContext;

        public RevenuesController(SalesDashBoardClient salesDashBoardClient, ILogger<RevenuesController> logger, IHubContext<SalesHub> hubContext)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
            _hubContext = hubContext;
        }


        [Authorize]
        [HttpGet("total-revenue/{year}")]
        public async Task<IActionResult> RevenueOfYear(int year)
        {
            try
            {
                _logger.LogInformation("fetching revenue data of a year");
                var revenueData = await _salesDashBoardClient.TotalRevenueAsync(year);

                return Ok(revenueData);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching total revenue of a year");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("total-revenue/{startYear}/{endYear}")]
        public async Task<IActionResult> RevenueBetweenYears(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching revenue data in a range of years");
                var revenueData = await _salesDashBoardClient.TotalRevenue2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveRevenue", revenueData);
                return Ok(revenueData);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching total revenue data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("revenue-per-order/{year}")]
        public async Task<IActionResult> RevenuePerOrderOfYear(int year)
        {
            try
            {
                _logger.LogInformation("Fetching revenue per order of a year");
                var revenuePerOrder = await _salesDashBoardClient.RevenuePerOrderAsync(year);

                return Ok(revenuePerOrder);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching revenue per order");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("revenue-per-order/{startYear}/{endYear}")]
        public async Task<IActionResult> RevenuePerOrderBetweenYears(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching revenue per order data in range of years");
                var revenuePerOrder = await _salesDashBoardClient.RevenuePerOrder2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveRevenuePerOrder", revenuePerOrder);
                return Ok(revenuePerOrder);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching revenue per order");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("revenue-growth-rate/{year}")]
        public async Task<IActionResult> RevenueGrowthRateOfYear(int year)
        {
            try
            {
                _logger.LogInformation("Fetching revenue growth rate of a year");
                var revenueGrowthRate = await _salesDashBoardClient.RevenueGrowthRateAsync(year);

                return Ok(revenueGrowthRate);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching revenue growth rate");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("revenue-growth-rate/{startYear}/{endYear}")]
        public async Task<IActionResult> RevenueGrowthRateInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching revenue growth rate in range of years");
                var revenueGrowthRate = await _salesDashBoardClient.RevenueGrowthRate2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveRevenueGrowthRate", revenueGrowthRate);
                return Ok(revenueGrowthRate);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching revenue growth rate");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("total-cost/{year}")]
        public async Task<IActionResult> TotalCostInvestedInYear(int year)
        {
            try
            {
                _logger.LogInformation("Fetching cost invested in a year");
                var totalCost = await _salesDashBoardClient.TotalCostAsync(year);

                return Ok(totalCost);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching total cost");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("total-cost/{startYear}/{endYear}")]
        public async Task<IActionResult> TotalCostInvestedInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching total cost invested in range of years");
                var totalCost = await _salesDashBoardClient.TotalCost2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveCost", totalCost);
                return Ok(totalCost);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching total cost");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("cost-per-order/{year}")]
        public async Task<IActionResult> CostPerOrder(int year)
        {
            try
            {
                _logger.LogInformation("Fetching cost per order in a year");
                var costPerOrder = await _salesDashBoardClient.CostPerOrderAsync(year);

                return Ok(costPerOrder);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching Cost per order");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("cost-per-order/{startYear}/{endYear}")]
        public async Task<IActionResult> CostPerOrderInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching cost per order in a range of years");
                var costPerOrder = await _salesDashBoardClient.CostPerOrder2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveCostPerOrder", costPerOrder);

                return Ok(costPerOrder);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching cost per order");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }
    }
}
