using FilamentsAPI.Model.Filaments;
using Microsoft.AspNetCore.Mvc;

namespace FilamentsAPI.Services
{
    /// <summary>
    /// Service methods for manipulating filaments
    /// </summary>
    public interface IFilamentsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<FilamentDetailsModel> CreateFilament(FilamentDetailsModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filamentId"></param>
        /// <returns></returns>
        Task<bool> DeleteFilament(int filamentId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filamentId"></param>
        /// <returns></returns>
        Task<FilamentDetailsModel> GetFilament(int filamentId);

        /// <summary>
        /// Returns the list of filaments that match the specified search criteria.
        /// </summary>
        /// <returns></returns>
        Task<List<FilamentHeaderModel>> SearchFilaments(string? queryString, int? boxId, string? brand, string? kind, string? color);

        /// <summary>
        /// Returns the list of all filaments .
        /// </summary>
        /// <returns></returns>
        Task<List<FilamentHeaderModel>> SearchFilaments();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<FilamentDetailsModel> UpdateFilament(FilamentDetailsModel model);

        /// <summary>
        /// Updates the photo for a filament.
        /// </summary>
        /// <param name="filamentId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<FilamentDetailsModel> UpdateFilamentPhoto(int filamentId, IFormFile file);
    }
}
