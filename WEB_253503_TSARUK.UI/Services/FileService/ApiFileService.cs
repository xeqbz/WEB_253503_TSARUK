using WEB_253503_TSARUK.UI.Services.Authentication;

namespace WEB_253503_TSARUK.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            if (formFile == null)
                return null;
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            request.Content = content;  

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            
            return String.Empty;
        }

        public async Task DeleteFileAsync(string fileUri)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(fileUri)
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting file: {response.ReasonPhrase}");
        }
    }
}
