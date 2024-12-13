using WEB_253503_TSARUK.Domain.Entities;

namespace WEB_253503_TSARUK.BlazorWasm.Services
{
	public interface IDataService
	{
		event Action DataLoaded;
		List<Category> Categories { get; set;  }
		List<Jewelry> Jewelries { get; set; }
		bool Success { get; set; }
		string ErrorMessage { get; set; }
		int TotalPages {  get; set; }
		int CurrentPage { get; set; }
		Category SelectedCategory { get; set; }
		public Task GetProductListAsync(int pageNo = 1);
		public Task GetCategoryListAsync();
		public Task GetAllProductsAsync();
	}
}
