using System.Runtime.CompilerServices;
using WEB_253503_TSARUK.API.Data;
using WEB_253503_TSARUK.Domain.Models;
using WEB_253503_TSARUK.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WEB_253503_TSARUK.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>>GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
