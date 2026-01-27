namespace HotelApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;
    using Web.ViewModels.Category;

    [AllowAnonymous]
    public class CategoryApiController : BaseExternalApiController
    {
        private readonly ICategoryService categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Produces("application/json")]
        [HttpGet("AllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AllCategoriesIndexViewModel>>> GetAllCategories()
        {
            var allCategories = await this.categoryService.GetAllCategoriesAsync();

            return Ok(allCategories);
        }

    }
}

