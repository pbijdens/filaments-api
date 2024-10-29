using FilamentsAPI.Model.Filaments;
using FilamentsAPI.Model.Storageboxes;
using FilamentsAPI.Services;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace FilamentsAPI.Persistence
{
    /// <summary>
    /// Represents either a storage box or a location where one or more roles of filament can
    /// be stored.
    /// </summary>
    public class StorageboxEntity
    {
        /// <summary>
        /// DB ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The name of this storage box.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// If there are any notes, they can be added here.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// When was the dessicant in this box last changed?
        /// </summary>
        public DateTimeOffset LastDessicantChange { get; set; } = DateTimeOffset.MinValue;

        /// <summary>
        /// Unique ID of the uploaded photo, if any.
        /// </summary>
        public string PhotoID { get; set; } = string.Empty;

        /// <summary>
        /// List of all filaments that are recorded as havign been stored in this box.
        /// </summary>
        public virtual List<FilamentEntity> Filaments { get; set; } = [];

        internal async Task<StorageboxHeaderModel> AsStorageboxHeaderModel(IPhotoStore photoStore)
        {
            StorageboxHeaderModel result = new()
            {
                Id = Id,
                Name = Name,
                LastDessicantChange = LastDessicantChange.ToString("yyyy-MM-dd"),
                Notes = Notes,
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

        internal StorageboxDetailsModel ToModel()
        {
            StorageboxDetailsModel result = new()
            {
                Id = Id,
                Name = Name,
                LastDessicantChange = LastDessicantChange.ToString("yyyy-MM-dd"),
                Notes = Notes,
                PhotoID = PhotoID,
            };
            return result;
        }
    }
}
