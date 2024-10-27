using Microsoft.AspNetCore.Mvc;
using FilamentsAPI.Model.Storageboxes;
using FilamentsAPI.Attributes;

namespace StorageboxesAPI.Controllers
{
    [ApiController]
    [Route("storagebox")]
    public class StorageBoxController : ControllerBase
    {
        /// <summary>
        /// Returns the list of all storageboxes
        /// </summary>
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<List<StorageboxHeaderModel>>> GetStorageboxes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns details for a single storagebox
        /// </summary>
        [HttpGet("{storageboxId}")]
        [Authorize]
        public async Task<ActionResult<StorageboxDetailsModel>> GetStoragebox([FromQuery] int storageboxId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update an existing storagebox
        /// </summary>
        [HttpPut()]
        [Authorize]
        public async Task<ActionResult<StorageboxHeaderModel>> UpdateStoragebox([FromBody] StorageboxDetailsModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new storagebox
        /// </summary>
        [HttpPost()]
        [Authorize]
        public async Task<ActionResult<StorageboxHeaderModel>> CreateStoragebox([FromBody] StorageboxHeaderModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a storagebox
        /// </summary>
        [HttpDelete("{storageboxId}")]
        [Authorize]
        public async Task<ActionResult<StorageboxHeaderModel>> DeleteStoragebox([FromQuery] int storageboxId)
        {
            throw new NotImplementedException();
        }
    }
}
