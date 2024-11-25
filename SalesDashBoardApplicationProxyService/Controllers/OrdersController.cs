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
    public class OrdersController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<OrdersController> _logger;

        private readonly IHubContext<SalesHub> _hubContext;


        public OrdersController(SalesDashBoardClient salesDashBoardClient, ILogger<OrdersController> logger, IHubContext<SalesHub> hubContext)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
            _hubContext = hubContext;
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AllOrdersData()
        {
            try
            {
                _logger.LogInformation("Fetching All orders data");
                var orders = await _salesDashBoardClient.OrdersAllAsync();
                return Ok(orders);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching orders data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> UsersOrderData(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching all orders of a user");
                var orders = await _salesDashBoardClient.UserAsync(userId);
                return Ok(orders);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while Fetching orders data of a user");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }

                

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderAddDto orderAddDto)
        {
            try
            {
                _logger.LogInformation("Creating a new order");
                await _salesDashBoardClient.OrdersAsync(orderAddDto);


                var updatedOrdersYearly = await _salesDashBoardClient.TotalOrders2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveTotalOrders", updatedOrdersYearly);

                var updatedAOVYearly = await _salesDashBoardClient.Aov2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveAverageOrderValue", updatedAOVYearly);

                var updatedUsersCount = await _salesDashBoardClient.UserCountAsync(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveUsersCount", updatedUsersCount);

                var updatedOrderedUserCount = await _salesDashBoardClient.OrderedUser2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveOrderedUsersCount",updatedOrderedUserCount);

                var updatedUnitSold = await _salesDashBoardClient.UnitSold2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveUnitSold", updatedUnitSold);

                var updatedSalesGrowthRate = await _salesDashBoardClient.SalesGrowth2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveSalesGrowthRate", updatedSalesGrowthRate);


                var updatedRevenue = await _salesDashBoardClient.TotalRevenue2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveRevenue", updatedRevenue);

                var updatedRevenuePerOrder = await _salesDashBoardClient.RevenuePerOrder2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveRevenuePerOrder", updatedRevenuePerOrder);

                var updatedRevenueGrowthRate = await _salesDashBoardClient.RevenueGrowthRate2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveRevenueGrowthRate", updatedRevenueGrowthRate);

                var updatedCost = await _salesDashBoardClient.TotalCost2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveCost", updatedCost);

                var updatedCostPerOrder = await _salesDashBoardClient.CostPerOrder2Async(2020, 2024);
                await _hubContext.Clients.All.SendAsync("ReceiveCostPerOrder", updatedCostPerOrder);

                return Ok();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while creating a new orders for a user");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }
    }
}
