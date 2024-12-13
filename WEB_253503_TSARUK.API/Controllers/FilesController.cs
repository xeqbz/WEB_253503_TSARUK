using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB_253503_TSARUK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
                return BadRequest("No file provided");

            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
                fileInfo.Delete();

            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);

            var host = HttpContext.Request.Host;
            var fileUrl = $"Https://{host}/Images/{file.FileName }";

            return Ok(fileUrl);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var filePath = Path.Combine (_imagePath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok("File deleted");
            }

            return NotFound("File not found");
        }
    }
}
