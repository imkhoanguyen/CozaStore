using CozaStore.Entities;
using CozaStore.Helpers;

namespace CozaStore.ViewModels
{
    public class SubCategoryVM
    {
        public PagedList<SubCategory>? SubCategories { get; set; }
        public SubCategory? SubCategory { get; set; }
        public int CategoryId { get; set; }
    }
}
