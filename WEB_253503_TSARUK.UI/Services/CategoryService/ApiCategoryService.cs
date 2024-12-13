using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;
using WEB_253503_TSARUK.UI.Models;
using WEB_253503_TSARUK.UI.Services.JewelryService;

namespace WEB_253503_TSARUK.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private HttpClient _httpClient;
        private string? _pageSize;
        private JsonSerializerOptions _serializeOptions;
        private ILogger<ApiJewelryService> _logger;
        private IConfiguration _configuration;

        public ApiCategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiJewelryService> logger, IOptions<UriData> uriDataOptions)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(uriDataOptions.Value.ApiUri);
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}categories/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<Category>>
                    {
                        Successfull = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<List<Category>>
            {
                Successfull = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }
    }
}
