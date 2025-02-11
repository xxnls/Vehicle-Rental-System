using API.Context;
using API.Models.DTOs.FileSystem;
using API.Models.DTOs.Other;
using API.Models.FileSystem;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.FileSystem
{
    public class DocumentCategoriesService : BaseApiService<DocumentCategory, DocumentCategoryDto, DocumentCategoryDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public DocumentCategoriesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        // Override CreateAsync to handle parent-child relationships
        public override async Task<DocumentCategoryDto> CreateAsync(DocumentCategoryDto createDto)
        {
            var entity = MapToEntity(createDto);

            // Set base properties
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            _apiDbContext.DocumentCategories.Add(entity);
            await _apiDbContext.SaveChangesAsync();

            return MapSingleEntityToDto(entity);
        }

        // Build search query for DocumentCategory
        protected override Expression<Func<DocumentCategory, bool>> BuildSearchQuery(string search)
        {
            return dc =>
                dc.DocumentCategoryId.ToString().Contains(search) ||
                dc.Name.Contains(search) ||
                (dc.Description != null && dc.Description.Contains(search)) ||
                (dc.ParentCategoryId != null && dc.ParentCategoryId.ToString().Contains(search));
        }

        // Active filter for DocumentCategory
        protected override Expression<Func<DocumentCategory, bool>> GetActiveFilter(bool showDeleted)
        {
            return dc => dc.IsActive != showDeleted;
        }

        // Map DTO to Entity
        public override DocumentCategory MapToEntity(DocumentCategoryDto model)
        {
            return new DocumentCategory
            {
                DocumentCategoryId = model.DocumentCategoryId,
                ParentCategoryId = model.ParentCategoryId,
                Name = model.Name,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        // Map Entity to DTO
        public override Expression<Func<DocumentCategory, DocumentCategoryDto>> MapToDto()
        {
            return dc => new DocumentCategoryDto
            {
                DocumentCategoryId = dc.DocumentCategoryId,
                ParentCategoryId = dc.ParentCategoryId,
                Name = dc.Name,
                Description = dc.Description,
                CreatedDate = dc.CreatedDate,
                ModifiedDate = dc.ModifiedDate,
                DeletedDate = dc.DeletedDate,
                IsActive = dc.IsActive
            };
        }

        // Map a single entity to DTO
        public override DocumentCategoryDto MapSingleEntityToDto(DocumentCategory entity)
        {
            return new DocumentCategoryDto
            {
                DocumentCategoryId = entity.DocumentCategoryId,
                ParentCategoryId = entity.ParentCategoryId,
                Name = entity.Name,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        // Find entity by ID
        public override async Task<DocumentCategory> FindEntityById(int id)
        {
            return await _apiDbContext.DocumentCategories
                .Include(dc => dc.ParentCategory)
                .Include(dc => dc.InverseParentCategory)
                .FirstOrDefaultAsync(dc => dc.DocumentCategoryId == id);
        }

        // Include related entities (e.g., parent and child categories)
        protected override IQueryable<DocumentCategory> IncludeRelatedEntities(IQueryable<DocumentCategory> query)
        {
            return query
                .Include(dc => dc.ParentCategory)
                .Include(dc => dc.InverseParentCategory);
        }

        // Update entity with DTO data
        protected override void UpdateEntity(DocumentCategory entity, DocumentCategoryDto model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.ParentCategoryId = model.ParentCategoryId;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsActive = model.IsActive;
            entity.DeletedDate = model.IsActive ? null : model.DeletedDate;
        }

        // Override DeleteAsync to handle parent-child relationships
        public override async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
                return false;

            // Check if the category has child categories
            if (entity.InverseParentCategory.Any())
            {
                throw new InvalidOperationException("Cannot delete a category that has child categories.");
            }

            // Soft delete the category
            entity.IsActive = false;
            entity.DeletedDate = DateTime.UtcNow;

            await _apiDbContext.SaveChangesAsync();
            return true;
        }
    }
}