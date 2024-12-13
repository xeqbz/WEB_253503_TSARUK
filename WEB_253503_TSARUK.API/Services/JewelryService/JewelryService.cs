using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WEB_253503_TSARUK.API.Data;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.API.Services.JewelryService
{
    public class JewelryService : IJewelryService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;
        
        public JewelryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Jewelries.Include(j => j.Category).AsQueryable();
            var dataList = new ListModel<Jewelry>();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
                query = query.Where(d => d.Category.NormalizedName.Equals(categoryNormalizedName));

            var count = await query.CountAsync();
            if (count == 0)
                return ResponseData<ListModel<Jewelry>>.Success(dataList);

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Jewelry>>.Error("No such page");

            dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<ListModel<Jewelry>>.Success(dataList);
        }

        public async Task<ResponseData<Jewelry>> GetProductByIdAsync(int id)
        {
            var product = await _context.Jewelries.FindAsync(id);
            if (product == null)
                return ResponseData<Jewelry>.Error("Product not found");

            return ResponseData<Jewelry>.Success(product);
        }

        public async Task UpdateProductAsync(int id, Jewelry product)
        {
            if (id != product.Id)
                throw new ArgumentException("Product ID missmatch");

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Jewelries.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            _context.Jewelries.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry product)
        {
            _context.Jewelries.Add(product);
            await _context.SaveChangesAsync();
            return ResponseData<Jewelry>.Success(product);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return ResponseData<string>.Error("No file uploaded.");

            var imagePath = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            var fileName = $"{id}_{Path.GetFileName(formFile.FileName)}";
            var filePath = Path.Combine(imagePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await formFile.CopyToAsync(stream);

            var url = $"{Path.Combine("images", fileName)}";
            return ResponseData<string>.Success(url);
        }

        public async Task<ResponseData<List<Jewelry>>> GetAllProductsAsync()
        {
            var jewelries = await _context.Jewelries
                .Include(j => j.Category)
                .OrderBy(j => j.Id)
                .ToListAsync();

            if (jewelries.Count == 0)
                return ResponseData<List<Jewelry>>.Error("No jewelries found");

            return ResponseData<List<Jewelry>>.Success(jewelries);
        }
    }
}
