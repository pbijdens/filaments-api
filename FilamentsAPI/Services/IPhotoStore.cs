namespace FilamentsAPI.Services
{
    /// <summary>
    /// Service that's takign care of storing and retrieving photo's
    /// </summary>
    public interface IPhotoStore
    {
        /// <summary>
        /// Open a previously stored photo for reading.
        /// </summary>
        /// <param name="photoID">The ID of the photo.</param>
        /// <returns>A stream for the photo's contents and the content type of the photo.</returns>
        Task<(Stream data, string contentType)> Open(string photoID);

        /// <summary>
        /// Delete a photo from storage.
        /// </summary>
        /// <param name="photoID">The ID of the photo.</param>
        Task Delete(string photoID);

        /// <summary>
        /// Store this photo.
        /// </summary>
        /// <param name="contents">The contents ot the photo.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>The ID of the photo</returns>
        Task<string> Store(Stream contents, string contentType);

        /// <summary>
        /// Open the thumbnail
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        Task<Stream?> OpenThumbnail(string photoID);
    }
}
