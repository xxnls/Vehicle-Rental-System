using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController<TEntity, TCreateDto, TResponseDto> : ControllerBase
        where TEntity : class
        where TCreateDto : class
        where TResponseDto : class
    {
        protected readonly IBaseService<TEntity, TCreateDto, TResponseDto> _service;

        protected BaseApiController(IBaseService<TEntity, TCreateDto, TResponseDto> service)
        {
            _service = service;
        }

        // GET: api/[controller]
        [HttpGet]
        public virtual async Task<ActionResult<PaginatedResult<TResponseDto>>> GetAll(
            string? search,
            int page,
            bool showDeleted,
            DateTime? createdBefore,
            DateTime? createdAfter,
            DateTime? modifiedBefore,
            DateTime? modifiedAfter,
            int pageSize)
        {
            var entities = await _service.GetAllAsync(
                search, 
                page, 
                showDeleted, 
                createdBefore, 
                createdAfter, 
                modifiedBefore, 
                modifiedAfter,
                pageSize);
            return Ok(entities);
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TResponseDto>> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        // POST: api/[controller]
        [HttpPost]
        public virtual async Task<ActionResult<TResponseDto>> Create(TCreateDto createDto)
        {
            var created = await _service.CreateAsync(createDto);
            return Created($"api/{ControllerName}/{{id}}", created);
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TResponseDto>> Update(int id, TCreateDto entity)
        {
            //if (id != GetEntityId(entity))
            //    return BadRequest(); NAPRAWIC

            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // This method should be implemented in derived controllers to extract the ID from the entity
        protected abstract int GetEntityId(TCreateDto entity);

        // Helper property to get controller name without "Controller" suffix
        protected virtual string ControllerName =>
            GetType().Name.Replace("Controller", string.Empty);
    }
}
