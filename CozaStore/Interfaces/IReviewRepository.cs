using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.Interfaces
{
	public interface IReviewRepository
	{
		Task<PagedList<Review>> GetAllReviewsAsync(int productId, PaginationParams paginationParams);

		void Create(Review review);

		Task<Review?> GetReviewAsync(int reviewId);
	}
}
