using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.API.Services.JewelryService
{
    public interface IJewelryService
    {
        Task<ResponseData<ListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        Task<ResponseData<Jewelry>> GetProductByIdAsync(int id);
        Task UpdateProductAsync(int id, Jewelry product);
        Task DeleteProductAsync(int id);
        Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry product);
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
        Task<ResponseData<List<Jewelry>>> GetAllProductsAsync();
    }
}
