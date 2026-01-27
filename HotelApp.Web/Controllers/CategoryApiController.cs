namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;

    [AllowAnonymous]
    public class CategoryApiController : BaseInternalApiController
    {
        private readonly ICategoryService categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("All")]
        public async Task<ActionResult> All()
        {
            var categories = await this.categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
