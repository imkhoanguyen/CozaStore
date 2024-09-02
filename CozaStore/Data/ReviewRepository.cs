using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly DataContext _context;

		public ReviewRepository(DataContext context)
        {
			_context = context;
		}
        public async Task<PagedList<Review>> GetAllReviewsAsync(int productId, PaginationParams paginationParams)
		{
			var query = _context.Reviews.AsQueryable();
			return await PagedList<Review>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
		}
	}
}
