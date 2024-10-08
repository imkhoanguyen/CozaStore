﻿using CozaStore.Models;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Task<Category?> GetCategoryAsync(int id);
        Task<PagedList<Category>> GetAllCategoriesAsync(CategoryParams categoryParams);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
