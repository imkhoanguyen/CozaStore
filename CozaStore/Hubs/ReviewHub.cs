using CozaStore.DTOs;
using CozaStore.Models;
using Microsoft.AspNetCore.SignalR;

namespace CozaStore.Hubs
{
    public class ReviewHub : Hub
    {
        public async Task SendReview(Review review)
        {
            await Clients.All.SendAsync("AddReview", review);
        }
    }
}
