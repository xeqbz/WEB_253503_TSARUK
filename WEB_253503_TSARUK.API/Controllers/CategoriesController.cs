using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.API.Data;
using WEB_253503_TSARUK.API.Services.CategoryService;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var result = await _categoryService.GetCategoryListAsync();
            return Ok(result);
        }
    }
}
