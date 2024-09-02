using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.Interfaces
{
	public interface IReviewRepository
	{
		Task<PagedList<Review>> GetAllReviewsAsync(int productId, PaginationParams paginationParams);
	}
}
