using FilamentsAPI.Attributes;
using FilamentsAPI.Model.Filaments;
using FilamentsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilamentsAPI.Controllers
{
    [ApiController]
    [Route("filament")]
    public class FilamentController : ControllerBase
    {
        /// <summary>
        /// Returns the list of all filaments
        /// </summary>
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<List<FilamentHeaderModel>>> GetFilaments()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns details for a single filament
        /// </summary>
        [HttpGet("{filamentId}")]
        [Authorize]
        public async Task<ActionResult<FilamentDetailsModel>> GetFilament([FromQuery] int filamentId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update an existing filament
        /// </summary>
        [HttpPut()]
        [Authorize]
        public async Task<ActionResult<FilamentHeaderModel>> UpdateFilament([FromBody] FilamentDetailsModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new filament
        /// </summary>
        [HttpPost()]
        [Authorize]
        public async Task<ActionResult<FilamentHeaderModel>> CreateFilament([FromBody] FilamentHeaderModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a filament
        /// </summary>
        [HttpDelete("{filamentId}")]
        [Authorize]
        public async Task<ActionResult<FilamentHeaderModel>> DeleteFilament([FromQuery] int filamentId)
        {
            throw new NotImplementedException();
        }
    }
}
