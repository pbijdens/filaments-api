using FilamentsAPI.Model.Filaments;
using FilamentsAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace FilamentsAPI.Services.Filaments
{
    /// <summary>
    /// Service for manipulating filament entities.
    /// </summary>
    public class FilamentsService(IPhotoStore photoStore, IConfiguration configuration) : IFilamentsService
    {
        /// <inheritdoc/>
        public async Task<FilamentDetailsModel> CreateFilament(FilamentDetailsModel model)
        {
            using FilamentsAPIDbContext db = new(configuration);
            FilamentEntity entity = new();

            await UpdateEntityFromModel(entity, db, model);

            EntityEntry<FilamentEntity> createdEntityEntry = await db.Filaments.AddAsync(entity);
            await db.SaveChangesAsync();
            int createdObjectId = createdEntityEntry.Entity?.Id ?? -1;
            return (await db.Filaments.Include(x => x.StorageBox).SingleAsync(x => x.Id == createdObjectId)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<FilamentDetailsModel> GetFilament(int filamentId)
        {
            using FilamentsAPIDbContext db = new(configuration);
            return (await db.Filaments.Include(x => x.StorageBox).SingleAsync(x => x.Id == filamentId)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<List<FilamentHeaderModel>> SearchFilaments()
        {
            using FilamentsAPIDbContext db = new(configuration);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var filaments = await db.Filaments
                .Include(x => x.StorageBox)
                .AsNoTracking()
                .OrderBy(x => x.StorageBox.Name) // null ref be here, database does not care
                .ThenBy(x => x.Description)
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            List<FilamentHeaderModel> result = new();
            foreach (var filament in filaments)
            {
                result.Add(await filament.AsFilamentHeaderModel(photoStore));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<List<FilamentHeaderModel>> SearchFilaments(string? queryString, int? boxId, string? brand, string? kind, string? color)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var query = db.Filaments.Include(f => f.StorageBox).AsNoTracking();
            if (!string.IsNullOrEmpty(queryString)) query = query.Where(x =>
                (x.Description != null && x.Description.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                || (x.Brand != null && x.Brand.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                || (x.Kind != null && x.Kind.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                || (x.Notes != null && x.Notes.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                || (x.StorageBox != null && x.StorageBox.Name.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                );
            if (boxId.HasValue) query = query.Where(x => x.StorageBox != null && x.StorageBox.Id == boxId);
            if (!string.IsNullOrEmpty(brand)) query = query.Where(x => x.Brand != null && x.Brand.Contains(brand));
            if (!string.IsNullOrEmpty(kind)) query = query.Where(x => x.Kind != null && x.Brand.Contains(kind));

#pragma warning disable CS8602 // Dereference of a possibly null reference, do not care for linq2sql
            query = query.OrderBy(x => x.StorageBox.Name).ThenBy(x => x.Description);
#pragma warning restore CS8602 

            List<FilamentHeaderModel> result = new();
            foreach (var filament in query.ToList())
            {
                result.Add(await filament.AsFilamentHeaderModel(photoStore));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<FilamentDetailsModel> UpdateFilament(FilamentDetailsModel model)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var entity = (await db.Filaments.FirstOrDefaultAsync(x => x.Id == model.Id)) ?? throw new ArgumentException("Can't find model", nameof(model));
            await UpdateEntityFromModel(entity, db, model);
            await db.SaveChangesAsync();
            return (await db.Filaments.Include(x => x.StorageBox).SingleAsync(x => x.Id == model.Id)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<FilamentDetailsModel> UpdateFilamentPhoto(int filamentId, IFormFile file)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var entity = (await db.Filaments.FirstOrDefaultAsync(x => x.Id == filamentId)) ?? throw new ArgumentException("Can't find model", nameof(filamentId));

            using Stream stream = file.OpenReadStream();
            string newId = await photoStore.Store(stream, file.ContentType);
            if (!string.IsNullOrEmpty(entity.PhotoID))
            {
                await photoStore.Delete(entity.PhotoID);
            }
            entity.PhotoID = newId;

            await db.SaveChangesAsync();
            return (await db.Filaments.Include(x => x.StorageBox).SingleAsync(x => x.Id == filamentId)).ToModel();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteFilament(int filamentId)
        {
            using FilamentsAPIDbContext db = new(configuration);
            var entity = (await db.Filaments.FirstOrDefaultAsync(x => x.Id == filamentId)) ?? throw new ArgumentException("Can't find model", nameof(filamentId));
            db.Filaments.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        private static async Task UpdateEntityFromModel(FilamentEntity entity, FilamentsAPIDbContext db, FilamentDetailsModel model)
        {
            entity.Brand = model.Brand;
            entity.Color1 = model.Color1;
            entity.Color2 = model.Color2;
            entity.Description = model.Description;
            entity.FirstAdded = string.IsNullOrWhiteSpace(model.FirstAdded) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(model.FirstAdded + "T00:00:00Z");
            entity.LastUpdated = string.IsNullOrWhiteSpace(model.LastUpdated) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(model.LastUpdated + "T00:00:00Z");
            entity.InitialWeight = model.InitialWeight;
            entity.Kind = model.Kind;
            entity.Weight = model.Weight;
            entity.PricePerKG = model.PricePerKG;
            entity.Notes = model.Notes;
            entity.StorageBox = await db.Storageboxes.FirstOrDefaultAsync(x => x.Id == model.StorageBoxID);
        }
    }
}
