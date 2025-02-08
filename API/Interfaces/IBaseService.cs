using API.Models;

namespace API.Interfaces
{
    public interface IBaseService<TEntity, TCreateDto, TResponseDto>
    {
        Task<PaginatedResult<TResponseDto>> GetAllAsync(
            string search = null,
            int page = 1,
            bool showDeleted = false,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null,
            int pageSize = 10,
            IQueryable<TEntity>? preFilteredQuery = null);
        Task<TResponseDto> GetByIdAsync(int id);
        Task<TResponseDto> CreateAsync(TCreateDto entity);
        Task<TResponseDto> UpdateAsync(int id, TCreateDto entity);
        Task<bool> DeleteAsync(int id);
    }

}
