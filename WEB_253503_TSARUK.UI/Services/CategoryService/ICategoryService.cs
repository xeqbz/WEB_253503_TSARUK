using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
