using FilamentsAPI.Attributes;
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
    public class FilamentController(IFilamentsService filamentsService) : ControllerBase
    {
        /// <summary>
        /// Returns the list of all filaments
        /// </summary>
        [HttpGet()]
        //[Authorize]
        public async Task<ActionResult<List<FilamentHeaderModel>>> GetFilaments()
        {
            return await filamentsService.GetFilaments();
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
        [HttpPut("{filamentId}/photo")]
        //[Authorize]
        public async Task<ActionResult<FilamentDetailsModel>> UpdateFilamentPhoto([FromRoute] int filamentId, IFormFile file)
        {
            return await filamentsService.UpdateFilamentPhoto(filamentId, file);
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
    }
}
