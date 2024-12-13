using Microsoft.Extensions.Options;
using WEB_253503_TSARUK.UI.HelperClasses;
using WEB_253503_TSARUK.UI.Models;
using WEB_253503_TSARUK.UI.Services.Authentication;
using WEB_253503_TSARUK.UI.Services.CategoryService;
using WEB_253503_TSARUK.UI.Services.FileService;
using WEB_253503_TSARUK.UI.Services.JewelryService;

namespace WEB_253503_TSARUK.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));

            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>((serviceProvider, opt) =>
            {
                var uriData = serviceProvider.GetRequiredService<IOptions<UriData>>().Value;
                opt.BaseAddress = new Uri(uriData.ApiUri);
            });

            builder.Services.AddHttpClient<IJewelryService, ApiJewelryService>((serviceProvider, opt) =>
            {
                var uriData = serviceProvider.GetRequiredService<IOptions<UriData>>().Value;
                opt.BaseAddress = new Uri(uriData.ApiUri);
            });

            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri(builder.Configuration["UriData:ApiUri"] + "Files"));

            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("KeyCloak"));

            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

            builder.Services.AddScoped<IAuthService, KeycloakAuthService>();
        }
    }
}
