using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.API.Data;
using WEB_253503_TSARUK.API.Services.JewelryService;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelriesController : ControllerBase
    {
        private readonly IJewelryService _jewelryService;

        public JewelriesController(IJewelryService jewelryService)
        {
            _jewelryService = jewelryService;
        }

        [HttpGet("categories/{categoryName?}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<List<Jewelry>>>> GetJewelries(string? categoryName, int pageNo = 1, int pageSize = 3)
        {
            var result = await _jewelryService.GetProductListAsync(categoryName, pageNo, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Jewelry>>> GetJewelry(int id)
        {
            var result = await _jewelryService.GetProductByIdAsync(id);
            if (!result.Successfull || result.Data == null)
                return NotFound(result.ErrorMessage);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutJewelry(int id, Jewelry jewelry)
        {
            if (id != jewelry.Id)
                return BadRequest("Jewelry ID mismatch");

            await _jewelryService.UpdateProductAsync(id, jewelry);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Jewelry>>> PostJewelry(Jewelry jewelry)
        {
            var result = await _jewelryService.CreateProductAsync(jewelry);
            return CreatedAtAction(nameof(GetJewelry), new { id = result.Data.Id }, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteJewelry(int id)
        {
            await _jewelryService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<ResponseData<List<Jewelry>>>> GetAllJewelries()
        {
            var response = await _jewelryService.GetAllProductsAsync();
            if (!response.Successfull)
                return NotFound(response.ErrorMessage);

            return Ok(response);
        }
    }
}
