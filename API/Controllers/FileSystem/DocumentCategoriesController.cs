using API.Models.DTOs.FileSystem;
using API.Models.FileSystem;
using API.Services.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FileSystem
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentCategoriesController(DocumentCategoriesService service)
        : BaseApiController<DocumentCategory, DocumentCategoryDto, DocumentCategoryDto>(service)
    {
        protected override int GetEntityId(DocumentCategoryDto entity)
        {
            return entity.DocumentCategoryId;
        }
    }
}
