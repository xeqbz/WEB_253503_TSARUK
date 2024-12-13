namespace WEB_253503_TSARUK.UI.Services.Authentication
{
    public interface ITokenAccessor
    {
        /// <summary>
        /// Получени access-token
        /// </summary>
        Task<string> GetAccessTokenAsync();
        /// <summary>
        /// Добавление заголовка Authorizition : bearer
        /// </summary>
        /// <param name="httpClient">HttpLient, в который добавляется заголовок</param>
        /// <returns></returns>
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }
}
