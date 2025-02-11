using API.Context;
using API.Models.DTOs.FileSystem;
using API.Models.DTOs.Other;
using API.Models.FileSystem;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.FileSystem
{
    public class DocumentTypesService : BaseApiService<DocumentType, DocumentTypeDto, DocumentTypeDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public DocumentTypesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        // Override CreateAsync to set default values
        public override async Task<DocumentTypeDto> CreateAsync(DocumentTypeDto createDto)
        {
            var entity = MapToEntity(createDto);

            // Set base properties
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;

            _apiDbContext.DocumentTypes.Add(entity);
            await _apiDbContext.SaveChangesAsync();

            return MapSingleEntityToDto(entity);
        }

        // Build search query for DocumentType
        protected override Expression<Func<DocumentType, bool>> BuildSearchQuery(string search)
        {
            return dt =>
                dt.DocumentTypeId.ToString().Contains(search) ||
                dt.Name.Contains(search) ||
                (dt.Description != null && dt.Description.Contains(search)) ||
                dt.FileExtension.Contains(search) ||
                dt.MaxFileSizeMb.ToString().Contains(search);
        }

        // Active filter for DocumentType
        protected override Expression<Func<DocumentType, bool>> GetActiveFilter(bool showDeleted)
        {
            return dt => dt.IsActive != showDeleted;
        }

        // Map DTO to Entity

        #region Mapping

        public override DocumentType MapToEntity(DocumentTypeDto model)
        {
            return new DocumentType
            {
                DocumentTypeId = model.DocumentTypeId,
                Name = model.Name,
                Description = model.Description,
                FileExtension = model.FileExtension,
                MaxFileSizeMb = model.MaxFileSizeMb,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        // Map Entity to DTO
        public override Expression<Func<DocumentType, DocumentTypeDto>> MapToDto()
        {
            return dt => new DocumentTypeDto
            {
                DocumentTypeId = dt.DocumentTypeId,
                Name = dt.Name,
                Description = dt.Description,
                FileExtension = dt.FileExtension,
                MaxFileSizeMb = dt.MaxFileSizeMb,
                CreatedDate = dt.CreatedDate,
                ModifiedDate = dt.ModifiedDate,
                DeletedDate = dt.DeletedDate,
                IsActive = dt.IsActive
            };
        }

        // Map a single entity to DTO
        public override DocumentTypeDto MapSingleEntityToDto(DocumentType entity)
        {
            return new DocumentTypeDto
            {
                DocumentTypeId = entity.DocumentTypeId,
                Name = entity.Name,
                Description = entity.Description,
                FileExtension = entity.FileExtension,
                MaxFileSizeMb = entity.MaxFileSizeMb,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        #endregion

        // Find entity by ID
        public override async Task<DocumentType> FindEntityById(int id)
        {
            return await _apiDbContext.DocumentTypes
                .FirstOrDefaultAsync(dt => dt.DocumentTypeId == id);
        }


        // Update entity with DTO data
        protected override void UpdateEntity(DocumentType entity, DocumentTypeDto model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.FileExtension = model.FileExtension;
            entity.MaxFileSizeMb = model.MaxFileSizeMb;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsActive = model.IsActive;
            entity.DeletedDate = model.IsActive ? null : model.DeletedDate;
        }

        // Override DeleteAsync to handle soft delete
        public override async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
                return false;

            // Soft delete the document type
            entity.IsActive = false;
            entity.DeletedDate = DateTime.UtcNow;

            await _apiDbContext.SaveChangesAsync();
            return true;
        }
    }
}