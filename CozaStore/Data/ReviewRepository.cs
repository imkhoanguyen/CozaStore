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

		public void Create(Review review)
		{
			_context.Reviews.Add(review);
		}

		public async Task<PagedList<Review>> GetAllReviewsAsync(int productId, PaginationParams paginationParams)
		{
			var query = _context.Reviews.Include(x=>x.AppUser).AsQueryable();
			query = query.Where(x=>x.ProductId == productId);
			query = query.OrderByDescending(x=>x.Id);
			return await PagedList<Review>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
		}

        public async Task<Review?> GetReviewAsync(int reviewId)
        {
			return await _context.Reviews.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == reviewId && !x.IsDelete);
        }
    }
}
