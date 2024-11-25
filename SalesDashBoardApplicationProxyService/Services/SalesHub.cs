using Microsoft.AspNetCore.SignalR;

namespace SalesDashBoardApplicationProxyService.Services
{
    public class SalesHub : Hub
    {
        public async Task BroadcastTotalOrders(object totalOrders)
        {
            await Clients.All.SendAsync("ReceiveTotalOrders", totalOrders);
        }


        public async Task BroadcastAverageOrderValue(object averageOrderValue)
        {
            await Clients.All.SendAsync("ReceiveAverageOrderValue", averageOrderValue);
        }
                    

        public async Task BroadcastUsersCount(object usersCount)
        {
            await Clients.All.SendAsync("ReceiveUsersCount", usersCount);
        }


        public async Task BroadCastOrderedUsersCount(object orderedUsers)
        {
            await Clients.All.SendAsync("ReceiveOrderedUsersCount", orderedUsers);
        }


        public async Task BroadcastUnitSold(object unitSold)
        {
            await Clients.All.SendAsync("ReceiveUnitSold", unitSold);
        }

        public async Task BroadcastSalesGrowthRate(object salesGrowthRate)
        {
            await Clients.All.SendAsync("ReceiveSalesGrowthRate", salesGrowthRate);
        }


        public async Task BroadcastRevenue(object revenueData)
        {
            await Clients.All.SendAsync("ReceiveRevenue", revenueData);
        }


        public async Task BroadcastRevenuePerOrder(object revenuePerOrder)
        {
            await Clients.All.SendAsync("ReceiveRevenuePerOrder", revenuePerOrder);
        }


        public async Task BroadcastRevenueGrowthRate(object  revenueGrowthRate)
        {
            await Clients.All.SendAsync("ReceiveRevenueGrowthRate", revenueGrowthRate);
        }


        public async Task BroadcastCost(object costData)
        {
            await Clients.All.SendAsync("ReceiveCost", costData);
        }


        public async Task BroadcastCostPerOrder(object costPerOrder)
        {
            await Clients.All.SendAsync("ReceiveCostPerOrder", costPerOrder);
        }
    }
}
