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
    public class SalesPerformancesController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<SalesPerformancesController> _logger;

        private readonly IHubContext<SalesHub> _hubContext;

        public SalesPerformancesController(SalesDashBoardClient salesDashBoardClient, ILogger<SalesPerformancesController> logger, IHubContext<SalesHub> hubContext)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
            _hubContext = hubContext;
        }



        [Authorize]
        [HttpGet("total-orders/{year}")]
        public async Task<IActionResult> TotalOrdersInYear(int year)
        {
            try
            {
                _logger.LogInformation("Fetching total orders data of a year");
                var totalOrders = await _salesDashBoardClient.TotalOrdersAsync(year);

                return Ok(totalOrders);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching total orders data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize] 
        [HttpGet("total-orders/{startYear}/{endYear}")]
        public async Task<IActionResult> TotalOrdersInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fteching total orders data in range of years");
                var totalOrders = await _salesDashBoardClient.TotalOrders2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveTotalOrders", totalOrders);
                return Ok(totalOrders);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching total orders data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("aov/{year}")]
        public async Task<IActionResult> AverageOrderValueInYear(int year)
        {
            try
            {
                _logger.LogInformation("Fteching avergae order value of a year");
                var averageOrderData = await _salesDashBoardClient.AovAsync(year);

                return Ok(averageOrderData);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching average order data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("aov/{startYear}/{endYear}")]
        public async Task<IActionResult> AverageOrderValueInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching avergae order value in range of year");
                var averageOrderData = await _salesDashBoardClient.Aov2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveAverageOrderValue", averageOrderData);
                return Ok(averageOrderData);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching average order data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("users-count/{year}")]
        public async Task<IActionResult> CountOfUsersInYear(int year)
        {
            try
            {
                _logger.LogInformation("Fetching users count in a year");
                var usersCount = await _salesDashBoardClient.UsersCountAsync(year);

                return Ok(usersCount);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching users count data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("user-count/{startYear}/{endYear}")]
        public async Task<IActionResult> CountOfUserInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching count of users in a range of years");
                var usersCount = await _salesDashBoardClient.UserCountAsync(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveUsersCount", usersCount);
                return Ok(usersCount);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching users count data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("ordered-user/{year}")]
        public async Task<IActionResult> OrderedUserCount(int year)
        {
            try
            {
                _logger.LogInformation("Fetching ordered user count in a year");
                var orderedUserCount = await _salesDashBoardClient.OrderedUserAsync(year);

                return Ok(orderedUserCount);                                                                                                                                    
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching ordered user data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("ordered-user/{startYear}/{endYear}")]
        public async Task<IActionResult> OrderedUserCountInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching ordered user count in a range of years");
                var orderesUserCount = await _salesDashBoardClient.OrderedUser2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveOrderedUsersCount", orderesUserCount);
                return Ok(orderesUserCount);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching ordered user data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("unit-sold/{year}")]
        public async Task<IActionResult> CountOfUnitSold(int year)
        {
            try
            {
                _logger.LogInformation("Fetching units sold data");
                var unitSoldData = await _salesDashBoardClient.UnitSoldAsync(year);

                return Ok(unitSoldData);        
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching unit sold data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpGet("unit-sold/{startYear}/{endYear}")]
        public async Task<IActionResult> CountOfUnitSoldInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching units sold data in a range");
                var unitSoldData = await _salesDashBoardClient.UnitSold2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveUnitSold", unitSoldData);
                return Ok(unitSoldData);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching unit sold data");
                return StatusCode(500, new { error = "Could not process due to some error" });                                      
            }
        }



        [Authorize]
        [HttpGet("sales-growth/{year}")]
        public async Task<IActionResult> SalesGrowthRateInYear(int year)
        {
            try
            {
                _logger.LogInformation("Fetching sales growth rate of a year");
                var salesGrowthRate = await _salesDashBoardClient.SalesGrowthAsync(year);

                return Ok(salesGrowthRate);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching sales growth data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("sales-growth/{startYear}/{endYear}")]
        public async Task<IActionResult> SalesGrowthInRange(int startYear, int endYear)
        {
            try
            {
                _logger.LogInformation("Fetching sales growth rate in range of years");
                var salesGrowthData = await _salesDashBoardClient.SalesGrowth2Async(startYear, endYear);

                await _hubContext.Clients.All.SendAsync("ReceiveSalesGrowthRate", salesGrowthData);
                return Ok(salesGrowthData);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching sales growth data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("popular-product/{year}")]
        public async Task<IActionResult> PopularProduct(int year)
        {
            try
            {
                _logger.LogInformation("Fetching popular product of the year");
                var popularProduct = await _salesDashBoardClient.PopularProductAsync(year);
                return Ok(popularProduct);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching popular product data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }
    }
}
