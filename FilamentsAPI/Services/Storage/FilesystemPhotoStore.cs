using FilamentsAPI.Model;
using FilamentsAPI.Services.Authentication;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FilamentsAPI.Services.Storage
{
    /// <summary>
    /// Photo storage backed by the local FS of the server running this software.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appSettings"></param>
    public class FilesystemPhotoStore(ILogger<FilesystemPhotoStore> logger, IOptions<AppSettings> appSettings) : IPhotoStore
    {
        /// <inheritdoc/>
        public async Task Delete(string photoID)
        {
            photoID = Regex.Replace(photoID.ToLowerInvariant(), "[^a-f0-9]", "");
            string filePath = Path.Combine(appSettings.Value.PhotoPath, photoID);
            File.Delete(filePath + ".bin");
            File.Delete(filePath + ".json");
            File.Delete(filePath + ".thumb.jpg");
            await Task.FromResult(0);

            logger.LogInformation("Deleted photo with ID {0}", photoID);
        }

        /// <inheritdoc/>
        public async Task<(Stream data, string contentType)> Open(string photoID)
        {
            photoID = Regex.Replace(photoID.ToLowerInvariant(), "[^a-f0-9]", "");
            string filePath = Path.Combine(appSettings.Value.PhotoPath, photoID);
            FileStream fileStream = new(filePath + ".bin", FileMode.Open, FileAccess.Read);
            PhotoMetadata metadata = JsonSerializer.Deserialize<PhotoMetadata>(await File.ReadAllTextAsync(filePath + ".json")) ?? new();

            logger.LogInformation("Opened photo with ID {0}", photoID);

            return (fileStream, metadata.ContentType);
        }

        /// <inheritdoc/>
        public async Task<Stream?> OpenThumbnail(string photoID)
        {
            photoID = Regex.Replace(photoID.ToLowerInvariant(), "[^a-f0-9]", "");
            string filePath = Path.Combine(appSettings.Value.PhotoPath, photoID);
            if (!File.Exists(filePath + ".thumb.jpg")) return null;

            FileStream fileStream = new(filePath + ".thumb.jpg", FileMode.Open, FileAccess.Read);
            return await Task.FromResult(fileStream);
        }

        /// <inheritdoc/>
        public async Task<string> Store(Stream contents, string contentType)
        {
            string photoID = TokenService.CalculateSha256Hash($"{Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()}");
            string filePath = Path.Combine(appSettings.Value.PhotoPath, photoID);
            using (FileStream fileStream = new(filePath + ".bin", FileMode.Create, FileAccess.Write))
            {
                await contents.CopyToAsync(fileStream);
            }
            await File.WriteAllTextAsync(filePath + ".json", JsonSerializer.Serialize(new PhotoMetadata { ContentType = contentType }));

            GenerateThumbnail(contents, photoID, filePath);

            logger.LogInformation("Created photo with ID {0}", photoID);
            return photoID;
        }

        private void GenerateThumbnail(Stream contents, string photoID, string filePath)
        {
            using (FileStream outStream = new(filePath + ".thumb.jpg", FileMode.Create, FileAccess.Write))
            {
                contents.Position = 0;
                using (Image image = Image.Load(contents))
                {
                    if (image.Height != 0 && image.Width != 0)
                    {
                        double scale = 256.0 / image.Width;
                        int width = (int)(image.Width * scale);
                        int height = (int)(image.Height * scale);
                        image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));
                    }
                    else
                    {
                        logger.LogWarning("Can't create thumbnail for photo with ID {0}, using original", photoID);
                    }
                    image.Save(outStream, new JpegEncoder());
                }
            }
        }

        /// <summary>
        /// Metadata
        /// </summary>
        public class PhotoMetadata
        {
            /// <summary></summary>
            public string ContentType { get; set; } = string.Empty;
        }
    }
}
