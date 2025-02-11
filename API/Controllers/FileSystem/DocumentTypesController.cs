using API.Models.DTOs.FileSystem;
using API.Models.FileSystem;
using API.Services.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FileSystem
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypesController(DocumentTypesService service)
        : BaseApiController<DocumentType, DocumentTypeDto, DocumentTypeDto>(service)
    {
        protected override int GetEntityId(DocumentTypeDto entity)
        {
            return entity.DocumentTypeId;
        }
    }
}
