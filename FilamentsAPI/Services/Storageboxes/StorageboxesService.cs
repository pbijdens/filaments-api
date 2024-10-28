using Microsoft.EntityFrameworkCore.ChangeTracking;
using FilamentsAPI.Model.Storageboxes;
using FilamentsAPI.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FilamentsAPI.Services.Storageboxes
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageboxesService(IPhotoStore photoStore, IConfiguration configuration) : IStorageboxesService
    {
        /// <inheritdoc/>
        public async Task<StorageboxDetailsModel> CreateStoragebox(StorageboxDetailsModel model)
        {
            using FilamentsAPIDbContext db = new(configuration);
            StorageboxEntity entity = new();

            await UpdateEntityFromModel(entity, db, model);

            EntityEntry<StorageboxEntity> createdEntityEntry = await db.Storageboxes.AddAsync(entity);
            await db.SaveChangesAsync();
            int createdObjectId = createdEntityEntry.Entity?.Id ?? -1;
            return (await db.Storageboxes.SingleAsync(x => x.Id == createdObjectId)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<StorageboxDetailsModel> GetStoragebox(int filamentId)
        {
            using FilamentsAPIDbContext db = new(configuration);
            return (await db.Storageboxes.SingleAsync(x => x.Id == filamentId)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<List<StorageboxHeaderModel>> GetStorageboxes()
        {
            using FilamentsAPIDbContext db = new(configuration);
            var filaments = await db.Storageboxes
                
                .AsNoTracking()
                .OrderBy(x => x.Name) // null ref be here, database does not care
                .ToListAsync();

            List<StorageboxHeaderModel> result = new();
            foreach (var filament in filaments)
            {
                result.Add(await filament.AsStorageboxHeaderModel(photoStore));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<StorageboxDetailsModel> UpdateStoragebox(StorageboxDetailsModel model)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var entity = (await db.Storageboxes.FirstOrDefaultAsync(x => x.Id == model.Id)) ?? throw new ArgumentException("Can't find model", nameof(model));
            await UpdateEntityFromModel(entity, db, model);
            await db.SaveChangesAsync();
            return (await db.Storageboxes.SingleAsync(x => x.Id == model.Id)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<StorageboxDetailsModel> UpdateStorageboxPhoto(int filamentId, IFormFile file)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var entity = (await db.Storageboxes.FirstOrDefaultAsync(x => x.Id == filamentId)) ?? throw new ArgumentException("Can't find model", nameof(filamentId));

            using Stream stream = file.OpenReadStream();
            string newId = await photoStore.Store(stream, file.ContentType);
            if (!string.IsNullOrEmpty(entity.PhotoID))
            {
                await photoStore.Delete(entity.PhotoID);
            }
            entity.PhotoID = newId;

            await db.SaveChangesAsync();
            return (await db.Storageboxes.SingleAsync(x => x.Id == filamentId)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteStoragebox(int filamentId)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var entity = (await db.Storageboxes.FirstOrDefaultAsync(x => x.Id == filamentId)) ?? throw new ArgumentException("Can't find model", nameof(filamentId));
            db.Storageboxes.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        private static async Task UpdateEntityFromModel(StorageboxEntity entity, FilamentsAPIDbContext db, StorageboxDetailsModel model)
        {
            entity.Name = model.Name;
            entity.Notes = model.Notes;
            entity.LastDessicantChange = DateTimeOffset.Parse(model.LastDessicantChange + "T00:00:00Z");
            await Task.FromResult(0);
        }
    }
}
