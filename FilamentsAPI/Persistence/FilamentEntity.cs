using FilamentsAPI.Model.Filaments;
using Microsoft.AspNetCore.Mvc;

namespace FilamentsAPI.Persistence
{
    /// <summary>
    /// Database representation of a filament record.
    /// </summary>
    public class FilamentEntity
    {
        /// <summary>
        /// DB ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Description of the filament.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// If there are any notes, they can be added here.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Brand of Filament.
        /// </summary>
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Kind of filament.
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// Current weight, including the spool.
        /// </summary>
        public int Weight { get; set; } = 0;

        /// <summary>
        /// How much did we pay for this particular filament per KG?
        /// </summary>
        public double PricePerKG { get; set; } = 0.0;

        /// <summary>
        /// Initial weight, including the spool.
        /// </summary>
        public int InitialWeight { get; set; } = 0;

        /// <summary>
        /// When was this first added?
        /// </summary>
        public DateTimeOffset FirstAdded { get; set; } = DateTimeOffset.MinValue;

        /// <summary>
        /// When was this last changed?
        /// </summary>
        public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.MinValue;

        /// <summary>
        /// Color 1 (HEX code)
        /// </summary>
        public string Color1 { get; set; } = string.Empty;

        /// <summary>
        /// Color 2 (HEX code)
        /// </summary>
        public string Color2 { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the uploaded photo, if any.
        /// </summary>
        public string PhotoID { get; set; } = string.Empty;

        /// <summary>
        /// The storagebox that this filament is or was last stored in.
        /// </summary>
        public virtual StorageboxEntity? StorageBox { get; set; }

        internal async Task<FilamentHeaderModel> AsFilamentHeaderModel(Services.IPhotoStore photoStore)
        {
            FilamentHeaderModel result = new()
            {
                Id = Id,
                Brand = Brand,
                Color1 = Color1,
                Color2 = Color2,
                Description = Description,
                FirstAdded = FirstAdded.ToString("yyyy-MM-dd"),
                LastUpdated = LastUpdated.ToString("yyyy-MM-dd"),
                InitialWeight = InitialWeight,
                Kind = Kind,
                Weight = Weight,
                PricePerKG = PricePerKG,
                Notes = Notes,
                StorageBoxID = StorageBox?.Id ?? -1,
                StorageBoxName = StorageBox?.Name ?? string.Empty,
            };

            if (!string.IsNullOrEmpty(PhotoID))
            {
                using var stream = await photoStore.OpenThumbnail(PhotoID);
                if (stream != null)
                {
                    result.Photo = "data:image/jpeg;base64," + stream.AsByteArray().Base64Encode();
                }
            }
            return result;
        }

        internal FilamentDetailsModel ToModel()
        {
            FilamentDetailsModel result = new()
            {
                Id = Id,
                Brand = Brand,
                Color1 = Color1,
                Color2 = Color2,
                Description = Description,
                FirstAdded = FirstAdded.ToString("yyyy-MM-dd"),
                LastUpdated = LastUpdated.ToString("yyyy-MM-dd"),
                InitialWeight = InitialWeight,
                Kind = Kind,
                Weight = Weight,
                PricePerKG = PricePerKG,
                Notes = Notes,
                StorageBoxID = StorageBox?.Id ?? -1,
                StorageBoxName = StorageBox?.Name ?? string.Empty,
                PhotoID = PhotoID,
            };
            return result;
        }
    }
}
