using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;
using WEB_253503_TSARUK.UI.Models;
using WEB_253503_TSARUK.UI.Services.Authentication;
using WEB_253503_TSARUK.UI.Services.FileService;

namespace WEB_253503_TSARUK.UI.Services.JewelryService
{
    public class ApiJewelryService : IJewelryService
    {
        private HttpClient _httpClient;
        private string? _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiJewelryService> _logger;
        private IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiJewelryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiJewelryService> logger, IOptions<UriData> uriDataOptions, IFileService fileService, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(uriDataOptions.Value.ApiUri);
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<ResponseData<ListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            string url = $"jewelries/categories/{categoryNormalizedName}?pageNo={pageNo}";
            var result = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Jewelry>>>(url);
            return result ?? new ResponseData<ListModel<Jewelry>>();
        }

        public async Task<ResponseData<ListModel<Jewelry>>> GetAllProductListAsync(string? categoryNormalizedName = null, int pageNo = 1, int pageSize = int.MaxValue)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            string url = $"jewelries/categories/{categoryNormalizedName}?pageNo={pageNo}&pageSize={pageSize}";
            var result = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Jewelry>>>(url);
            return result ?? new ResponseData<ListModel<Jewelry>>();
        }

        public async Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry product, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            product.Image = "images/no-image.jpg";

            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                    product.Image = imageUrl;
            }

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "jewelries");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Jewelry>>(_serializerOptions);
                return data;
            }

            _logger.LogError($"-----> object not created. Error: {response.StatusCode.ToString()}");
            return ResponseData<Jewelry>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
        }

        public async Task DeleteProductAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.DeleteAsync($"jewelries/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ResponseData<Jewelry>> GetProductByIdAsync(int id) 
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var result = await _httpClient.GetFromJsonAsync<ResponseData<Jewelry>>($"jewelries/{id}");
            return result ?? new ResponseData<Jewelry>();
        }

        public async Task UpdateProductAsync(int id, Jewelry jewelry, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            // Получаем текущий продукт, чтобы узнать текущее изображение
            var currentProductUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Jewelries/{id}");
            var currentProductResponse = await _httpClient.GetAsync(currentProductUri);

            if (!currentProductResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Failed to retrieve product with id {id}. Error: {currentProductResponse.StatusCode}");
                throw new Exception($"Unable to retrieve product with id {id}. Error: {currentProductResponse.StatusCode}");
            }

            var currentProduct = await currentProductResponse.Content.ReadFromJsonAsync<ResponseData<Jewelry>>(_serializerOptions);

            if (currentProduct == null || currentProduct.Data == null)
            {
                throw new Exception("Product not found");
            }

            var existingImage = currentProduct.Data.Image;

            // Обработка нового изображения
            if (formFile != null)
            {
                var newImageUrl = await _fileService.SaveFileAsync(formFile);

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    jewelry.Image = newImageUrl;

                    // Удаляем старое изображение, если оно не является "images/no-image.jpg"
                    if (!string.IsNullOrEmpty(existingImage) && existingImage != "images/no-image.jpg")
                    {
                        await _fileService.DeleteFileAsync(existingImage);
                    }
                }
            }
            else
            {
                // Если изображение не передано, сохраняем текущее
                jewelry.Image = existingImage;
            }

            // Обновляем продукт
            var updateProductUri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"jewelries/{id}");
            var updateResponse = await _httpClient.PutAsJsonAsync(updateProductUri, jewelry, _serializerOptions);

            if (!updateResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Failed to update product with id {id}. Error: {updateResponse.StatusCode}");
                throw new Exception($"Failed to update product. Error: {updateResponse.StatusCode}");
            }
        }

    }
}
