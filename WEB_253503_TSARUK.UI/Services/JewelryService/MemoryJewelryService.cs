//using WEB_253503_TSARUK.Domain.Entities;
//using WEB_253503_TSARUK.Domain.Models;
//using WEB_253503_TSARUK.UI.Services.CategoryService;
//using System.Configuration;
//using Newtonsoft.Json;

//namespace WEB_253503_TSARUK.UI.Services.JewelryService
//{
//    public class MemoryJewelryService : IJewelryService
//    {
//        List<Jewelry> _jewelry;
//        List<Category> _categories;
//        private readonly IConfiguration _configuration;
//        public MemoryJewelryService(ICategoryService categoryService, IConfiguration config)
//        {
//            _configuration = config;
//            _categories = categoryService.GetCategoryListAsync().Result.Data; 
//            SetupData();
//        }

//        /// <summary>
//        /// Инициализация списков
//        /// </summary>
//        private void SetupData()
//        {
//            _jewelry = new List<Jewelry>
//            {
//                new Jewelry
//                {
//                    Id = 1,
//                    Name = "Heart Ring",
//                    Description = "Hand-finished in 14k rose gold unique metal blend, this piece features a pink heart-shaped central, elevated stone.",
//                    Price = 10500,
//                    Image = "images/heart-ring.jpg",
//                    Category = _categories.Find(c=>c.NormalizedName.Equals("rings"))
//                },
//                new Jewelry
//                {
//                    Id = 2,
//                    Name = "Sparlking Band Ring",
//                    Description = "The pavé row sparkles from the outer front half of the band, which features a squared profile. Peek inside the band to see the engraved iconic Pandora logo.",
//                    Price = 4500,
//                    Image = "images/sparkling-ring.jpg",
//                    Category = _categories.Find(c=>c.NormalizedName.Equals("rings"))
//                },
//                new Jewelry
//                {
//                    Id = 3,
//                    Name = "Sparkling Hoop Earrings",
//                    Description = "An essential for every collection, these sterling silver hoop earrings are set with a row of clear cubic zirconia. Versatile and classic, these hoops make a winning gift or addition to your own look. ",
//                    Price = 5600,
//                    Image = "images/sparkling-ear.jpg",
//                    Category = _categories.Find(c=>c.NormalizedName.Equals("earrings"))
//                },
//                new Jewelry
//                {
//                    Id = 4,
//                    Name = "Elevated Heart Stud Earrings",
//                    Description = "These timeless and dreamy sterling silver earrings feature an elevated cubic zirconia setting for extra brilliance and sparkle. The sparkling ear studs are a classic style that will be treasured for years to come thanks to its sophisticated, yet simple design.",
//                    Price = 6000,
//                    Image = "images/heart-ear.jpg",
//                    Category = _categories.Find(c=>c.NormalizedName.Equals("earrings"))
//                },
//                new Jewelry
//                {
//                    Id = 5,
//                    Name = "Sparkling Tennis Bracelet",
//                    Description = "This sterling silver Sparkling Tennis Bracelet features squared collets and is decorated with a bold line of clear cubic zirconia to make a statement fit for any occasion.",
//                    Price = 11000,
//                    Image = "images/tennis-braclet.jpg",
//                    Category = _categories.Find(c=>c.NormalizedName.Equals("bracelets"))
//                },
//                new Jewelry
//                {
//                    Id = 6,
//                    Name = "Celestial Stars Bracelet",
//                    Description = "Our star bracelet features a collection of irregularly-shaped sterling silver star shapes embellished with round cubic zirconia stones bound together by pavé bars to evoke a life beyond your wildest dreams, with each star symbolizing a past milestone or a future achievement.",
//                    Price = 8800,
//                    Image = "images/star-braclet.jpg",
//                    Category = _categories.Find(c=>c.NormalizedName.Equals("bracelets"))
//                }
//            };
//        }

//        public async Task<ResponseData<ListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1)
//        {
//            var itemsPerPage = _configuration.GetValue<int>("PageSettings:ItemsPerPage");

//            var filteredJewelry = _jewelry.Where(j => categoryNormalizedName == null || (j.Category != null && j.Category.NormalizedName.Equals(categoryNormalizedName))).ToList();

//            int totalItems = filteredJewelry.Count;
//            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

//            var pagedJewelry = filteredJewelry.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList();

//            var result = new ListModel<Jewelry>
//            {
//                Items = pagedJewelry,
//                CurrentPage = pageNo,
//                TotalPages = totalPages
//            };
//            return ResponseData<ListModel<Jewelry>>.Success(result);
//        }


//        public Task<ResponseData<Jewelry>> GetProductByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateProductAsync(int id, Jewelry product, IFormFile? formFile)
//        {
//            throw new NotImplementedException();
//        }

//        public Task DeleteProductAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry product, IFormFile? formFile)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
