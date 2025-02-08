using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.Models;

namespace API.Services
{
    public abstract class BaseApiService<TEntity, TCreateDto, TResponseDto> : IBaseService<TEntity, TCreateDto, TResponseDto>
        where TEntity : class
        where TCreateDto : class
        where TResponseDto : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseApiService(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Build a query to search for entities based on a search string.
        /// </summary>
        /// <param name="search">
        /// The search string to filter entities by.
        /// </param>
        /// <returns>
        /// An expression to filter entities by the search string.
        /// </returns>
        protected abstract Expression<Func<TEntity, bool>> BuildSearchQuery(string search);

        /// <summary>
        /// Find an entity by its ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the entity to find.
        /// </param>
        /// <returns>
        /// The entity with the given ID, or null if not found.
        /// </returns>
        public abstract Task<TEntity> FindEntityById(int id);

        /// <summary>
        /// Map a DTO to an entity.
        /// </summary>
        /// <param name="dto">
        /// The DTO to map to an entity.
        /// </param>
        /// <returns>
        /// The entity created from the DTO.
        /// </returns>
        public abstract TEntity MapToEntity(TCreateDto dto);

        /// <summary>
        /// Map an entity to a DTO.
        /// </summary>
        /// <returns>
        /// An expression to map an entity to a DTO.
        /// </returns>
        public abstract Expression<Func<TEntity, TResponseDto>> MapToDto();

        /// <summary>
        /// Map a single entity to a DTO.
        /// </summary>
        /// <param name="entity">
        /// The entity to map to a DTO.
        /// </param>
        /// <returns>
        /// The DTO created from the entity.
        /// </returns>
        public abstract TResponseDto MapSingleEntityToDto(TEntity entity);

        /// <summary>
        /// Update an entity with data from a DTO.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <param name="dto">
        /// The DTO with data to update the entity with.
        /// </param>
        protected abstract void UpdateEntity(TEntity entity, TCreateDto dto);

        protected virtual IQueryable<TEntity> IncludeRelatedEntities(IQueryable<TEntity> query)
        {
            // Default implementation does nothing
            return query;
        }

        /// <summary>
        /// Get all entities with pagination and filtering.
        /// </summary>
        /// <returns>
        /// A paginated result of entities.
        /// </returns>
        public virtual async Task<PaginatedResult<TResponseDto>> GetAllAsync(
            string? search = null,
            int page = 1,
            bool showDeleted = false,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null,
            int pageSize = 10,
            IQueryable<TEntity>? preFilteredQuery = null
            )
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page and page size must be greater than 0.");
            }

            // Start with base query
            IQueryable<TEntity> query = preFilteredQuery ?? _dbSet;

            // Apply includes if specified
            query = IncludeRelatedEntities(query);

            // Apply active/deleted filter
            query = query.Where(GetActiveFilter(showDeleted));

            // Apply search if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(BuildSearchQuery(search));
            }

            // Apply date filters if implemented by the entity
            if (typeof(TEntity).GetInterface("IBaseModel") != null)
            {
                query = ApplyDateFilters(query, createdBefore, createdAfter, modifiedBefore, modifiedAfter);
            }

            // Get total count for pagination
            int totalItemCount = await query.CountAsync();

            // Get paginated items, order by primary key
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToDto())
                .ToListAsync();

            // Return paginated result
            return new PaginatedResult<TResponseDto>
            {
                Items = items,
                TotalItemCount = totalItemCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Get an entity by its ID and map it to DTO.
        /// </summary>
        /// <param name="id">
        /// The ID of the entity to get.
        /// </param>
        /// <returns>
        /// The DTO of the entity with the given ID.
        /// </returns>
        public virtual async Task<TResponseDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        /// <summary>
        /// Create a new entity from a DTO.
        /// </summary>
        /// <param name="createDto">
        /// The DTO to create the entity from.
        /// </param>
        /// <returns>
        /// The DTO of the created entity.
        /// </returns>
        public virtual async Task<TResponseDto> CreateAsync(TCreateDto createDto)
        {
            var entity = MapToEntity(createDto);

            switch (entity)
            {
                // Set base model properties if implemented
                case IBaseModel baseModel:
                    baseModel.CreatedDate = DateTime.UtcNow;
                    baseModel.IsActive = true;
                    break;
                case ILocation baseLocation:
                    baseLocation.IsActive = true;
                    baseLocation.DateTime = DateTime.UtcNow;
                    break;
            }

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return MapSingleEntityToDto(entity);
        }

        /// <summary>
        /// Update an entity with data from a DTO.
        /// </summary>
        /// <param name="id">
        /// The ID of the entity to update.
        /// </param>
        /// <param name="updateDto">
        /// The DTO with data to update the entity with.
        /// </param>
        /// <returns>
        /// The DTO of the updated entity.
        /// </returns>
        public virtual async Task<TResponseDto> UpdateAsync(int id, TCreateDto updateDto)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity).Name} not found");
            }

            UpdateEntity(entity, updateDto);

            switch (entity)
            {
                case IBaseModel baseModel:
                    baseModel.ModifiedDate = DateTime.UtcNow;
                    break;
                case ILocation baseLocation:
                    baseLocation.DateTime = DateTime.UtcNow;
                    break;
            }

            await _context.SaveChangesAsync();

            return MapSingleEntityToDto(entity);
        }

        /// <summary>
        /// Delete an entity by its ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the entity to delete.
        /// </param>
        /// <returns>
        /// True if the entity was deleted, false if not found.
        /// </returns>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindEntityById(id);

            switch (entity)
            {
                case null:
                    return false;
                case ILocation baseLocation:
                    baseLocation.IsActive = false;
                    baseLocation.DateTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                case IBaseModel baseModel:
                    baseModel.DeletedDate = DateTime.UtcNow;
                    baseModel.IsActive = false;
                    await _context.SaveChangesAsync();
                    return true;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get the active filter for entities.
        /// </summary>
        /// <param name="showDeleted">
        /// Whether to show deleted entities.
        /// </param>
        /// <returns>
        /// An expression to filter entities by active status.
        /// </returns>
        protected virtual Expression<Func<TEntity, bool>> GetActiveFilter(bool showDeleted)
        {
            return entity => true; // This would return all entities, no filter.
        }

        /// <summary>
        /// Apply date filters to a query.
        /// </summary>
        /// <param name="query">
        /// The query to apply date filters to.
        /// </param>
        /// <returns>
        /// The query with date filters applied.
        /// </returns>
        private IQueryable<TEntity> ApplyDateFilters(
            IQueryable<TEntity> query,
            DateTime? createdBefore,
            DateTime? createdAfter,
            DateTime? modifiedBefore,
            DateTime? modifiedAfter)
        {
            if (createdBefore.HasValue)
            {
                query = query.Where(e => ((IBaseModel)e).CreatedDate <= createdBefore.Value);
            }

            if (createdAfter.HasValue)
            {
                query = query.Where(e => ((IBaseModel)e).CreatedDate >= createdAfter.Value);
            }

            if (modifiedBefore.HasValue)
            {
                query = query.Where(e => ((IBaseModel)e).ModifiedDate <= modifiedBefore.Value);
            }

            if (modifiedAfter.HasValue)
            {
                query = query.Where(e => ((IBaseModel)e).ModifiedDate >= modifiedAfter.Value);
            }

            return query;
        }
    }
}
