using Microsoft.AspNetCore.Mvc;
using FilamentsAPI.Model.Storageboxes;
using FilamentsAPI.Services;

namespace FilamentsAPI.Controllers
{
    /// <summary>
    /// Endpoints to manipulate storage boxes.
    /// </summary>
    [ApiController]
    [Route("storagebox")]
    public class StorageBoxController(IStorageboxesService storageboxesService, IPhotoStore photoStore) : ControllerBase
    {
        /// <summary>
        /// Returns the list of all storageboxes
        /// </summary>
        [HttpGet()]
        //[Authorize]
        public async Task<ActionResult<List<StorageboxHeaderModel>>> GetStorageboxes()
        {
            return await storageboxesService.GetStorageboxes();
        }

        /// <summary>
        /// Returns details for a single storagebox
        /// </summary>
        [HttpGet("{storageboxId}")]
        //[Authorize]
        public async Task<ActionResult<StorageboxDetailsModel>> GetStoragebox([FromRoute] int storageboxId)
        {
            return await storageboxesService.GetStoragebox(storageboxId);
        }

        /// <summary>
        /// Update an existing storagebox
        /// </summary>
        [HttpPut()]
        //[Authorize]
        public async Task<ActionResult<StorageboxDetailsModel>> UpdateStoragebox([FromBody] StorageboxDetailsModel model)
        {
            return await storageboxesService.UpdateStoragebox(model);
        }

        /// <summary>
        /// Create or update the filament photo
        /// </summary>
        [HttpPost("{storageboxId}/photo")]
        //[Authorize]
        public async Task<ActionResult<StorageboxDetailsModel>> UpdateFilamentPhoto([FromRoute] int storageboxId, IFormFile photo)
        {
            return await storageboxesService.UpdateStorageboxPhoto(storageboxId, photo);
        }

        /// <summary>
        /// Create a new storagebox
        /// </summary>
        [HttpPost()]
        //[Authorize]
        public async Task<ActionResult<StorageboxDetailsModel>> CreateStoragebox([FromBody] StorageboxDetailsModel model)
        {
            return await storageboxesService.CreateStoragebox(model);
        }

        /// <summary>
        /// Delete a storagebox
        /// </summary>
        [HttpDelete("{storageboxId}")]
        //[Authorize]
        public async Task<ActionResult<bool>> DeleteStoragebox([FromRoute] int storageboxId)
        {
            return await storageboxesService.DeleteStoragebox(storageboxId);
        }

        /// <summary>
        /// Gets the full photo
        /// </summary>
        /// <param name="storageboxId"></param>
        /// <returns></returns>
        [HttpGet("{storageboxId}/photo")]
        public async Task<FileStreamResult?> Photo([FromRoute] int storageboxId)
        {
            StorageboxDetailsModel storagebox = await storageboxesService.GetStoragebox(storageboxId);
            if (!string.IsNullOrWhiteSpace(storagebox?.PhotoID))
            {
                (Stream data, string contentType) photoStream = await photoStore.Open(storagebox.PhotoID);
                return new FileStreamResult(photoStream.data, photoStream.contentType);
            }
            return new FileStreamResult(new FileStream("noimage.png", FileMode.Open, FileAccess.Read), "image/png");
        }
    }
}
