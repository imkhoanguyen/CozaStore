using CozaStore.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace CozaStore.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrder(OrderDto orderDto)
        {
            await Clients.All.SendAsync("AddOrder", orderDto);
        }

        public async Task SendPaymentStatusSuccess(int orderId)
        {
            await Clients.All.SendAsync("PaymentSuccess", orderId);
        }
    }
}
