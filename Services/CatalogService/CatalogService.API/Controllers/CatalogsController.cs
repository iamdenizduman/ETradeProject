using CatalogService.Application.CategoryHandlers.AddCategoryCommand;
using CatalogService.Application.CategoryHandlers.DeactivateCategoryCommand;
using CatalogService.Application.CategoryHandlers.GetAllCategoriesQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(DeactivateCategory))]
        public async Task<IActionResult> DeactivateCategory(DeactivateCategoryRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response.Message);
            
            return Ok(response);
        }

        [HttpGet(nameof(GetAllCategoriesQuery))]
        public async Task<IActionResult> GetAllCategoriesQuery([FromQuery] GetAllCategoriesQuery request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpPost(nameof(AddCategory))]
        public async Task<IActionResult> AddCategory(AddCategoryRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }
    }
}
