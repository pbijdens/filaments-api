using FilamentsAPI.Model.Storageboxes;
using Microsoft.AspNetCore.Mvc;

namespace FilamentsAPI.Services
{
    /// <summary>
    /// Service methods for manipulating filaments
    /// </summary>
    public interface IStorageboxesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<StorageboxDetailsModel> CreateStoragebox(StorageboxDetailsModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filamentId"></param>
        /// <returns></returns>
        Task<bool> DeleteStoragebox(int filamentId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filamentId"></param>
        /// <returns></returns>
        Task<StorageboxDetailsModel> GetStoragebox(int filamentId);

        /// <summary>
        /// Returns the list of filaments.
        /// </summary>
        /// <returns></returns>
        Task<List<StorageboxHeaderModel>> GetStorageboxes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<StorageboxDetailsModel> UpdateStoragebox(StorageboxDetailsModel model);

        /// <summary>
        /// Updates the photo for a filament.
        /// </summary>
        /// <param name="filamentId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<StorageboxDetailsModel> UpdateStorageboxPhoto(int filamentId, IFormFile file);
    }
}
