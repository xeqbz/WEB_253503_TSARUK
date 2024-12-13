//using WEB_253503_TSARUK.Domain.Entities;
//using WEB_253503_TSARUK.Domain.Models;

//namespace WEB_253503_TSARUK.UI.Services.CategoryService
//{
//    public class MemoryCategoryService : ICategoryService
//    {
//        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
//        {
//            var categories = new List<Category>()
//            {
//                new Category
//                {
//                    Id = 1,
//                    Name = "Серьги",
//                    NormalizedName = "earrings"
//                },
//                new Category
//                {
//                    Id = 2,
//                    Name = "Кольца",
//                    NormalizedName = "rings"
//                },
//                new Category
//                {
//                    Id = 3,
//                    Name = "Браслеты",
//                    NormalizedName = "bracelets"
//                }
//            };
//            var result = ResponseData<List<Category>>.Success(categories);
//            return Task.FromResult(result);
//        }
//    }
//}
