using FilamentsAPI.Model.Filaments;
using FilamentsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilamentsAPI.Controllers
{
    /// <summary>
    /// Filament-related operations
    /// </summary>
    [ApiController]
    [Route("filament")]
    public class FilamentController(IFilamentsService filamentsService, IPhotoStore photoStore) : ControllerBase
    {
        /// <summary>
        /// Returns the list of all filaments
        /// </summary>
        [HttpGet()]
        //[Authorize]
        public async Task<ActionResult<List<FilamentHeaderModel>>> GetFilaments()
        {
            return await filamentsService.SearchFilaments();
        }

        /// <summary>
        /// Returns details for a single filament
        /// </summary>
        [HttpGet("{filamentId}")]
        //[Authorize]
        public async Task<ActionResult<FilamentDetailsModel>> GetFilament([FromRoute] int filamentId)
        {
            return await filamentsService.GetFilament(filamentId);
        }

        /// <summary>
        /// Update an existing filament
        /// </summary>
        [HttpPut()]
        //[Authorize]
        public async Task<ActionResult<FilamentDetailsModel>> UpdateFilament([FromBody] FilamentDetailsModel model)
        {
            return await filamentsService.UpdateFilament(model);
        }

        /// <summary>
        /// Create a new filament
        /// </summary>
        [HttpPost()]
        //[Authorize]
        public async Task<ActionResult<FilamentDetailsModel>> CreateFilament([FromBody] FilamentDetailsModel model)
        {
            return await filamentsService.CreateFilament(model);
        }

        /// <summary>
        /// Create or update the filament photo
        /// </summary>
        [HttpPost("{filamentId}/photo")]
        //[Authorize]
        public async Task<ActionResult<FilamentDetailsModel>> UpdateFilamentPhoto([FromRoute] int filamentId, IFormFile photo)
        {
            return await filamentsService.UpdateFilamentPhoto(filamentId, photo);
        }

        /// <summary>
        /// Delete a filament
        /// </summary>
        [HttpDelete("{filamentId}")]
        //[Authorize]
        public async Task<ActionResult<bool>> DeleteFilament([FromRoute] int filamentId)
        {
            return await filamentsService.DeleteFilament(filamentId);
        }

        /// <summary>
        /// Gets the full photo
        /// </summary>
        /// <param name="filamentId"></param>
        /// <returns></returns>
        [HttpGet("{filamentId}/photo")]
        public async Task<FileStreamResult?> Photo([FromRoute] int filamentId)
        {
            FilamentDetailsModel filament = await filamentsService.GetFilament(filamentId);
            if (!string.IsNullOrWhiteSpace(filament?.PhotoID))
            {
                (Stream data, string contentType) photoStream = await photoStore.Open(filament.PhotoID);
                return new FileStreamResult(photoStream.data, photoStream.contentType);
            }
            return new FileStreamResult(new FileStream("noimage.png", FileMode.Open, FileAccess.Read), "image/png");
        }

        /// <summary>
        /// Returns the list of all filaments
        /// </summary>
        [HttpGet("search")]
        //[Authorize]
        public async Task<ActionResult<List<FilamentHeaderModel>>> Search(
            [FromQuery(Name = "q")] string? queryString, 
            [FromQuery(Name = "box")] int? boxId, 
            [FromQuery(Name = "brand")] string? brand, 
            [FromQuery(Name = "kind")] string? kind, 
            [FromQuery(Name = "color")] string? color)
        {
            return await filamentsService.SearchFilaments(queryString, boxId, brand, kind, color);
        }
    }
}
