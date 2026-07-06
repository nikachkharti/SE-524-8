using CloudinaryDotNet;
using Company524.Application.Contracts.Service;
using Company524.Application.Exceptions;
using Company524.Application.Models.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace Company524.Application.Service
{
    public class CloudinaryImageService(Cloudinary cloudinary) : ICloudinaryImageService
    {
        public async Task<bool> DeleteAsync(string publicId, bool invalidateCdn = true, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return false;

            var deletionParams = new CloudinaryDotNet.Actions.DeletionParams(publicId)
            {
                Invalidate = invalidateCdn
            };

            var result = await cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        public async Task<ImageUploadResultDto> UpdateAsync(string publicId, int width, int height, IFormFile newFile, bool invalidateCdn = true, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new BadRequestException("Public id is required while uploading a file on cloudinary");

            return await UploadInternalAsync
            (
                newFile,
                width,
                height,
                folder: null,
                overwrite: true,
                invalidateCdn: invalidateCdn,
                publicId: publicId,
                ct
            );
        }

        public async Task<ImageUploadResultDto> UploadAsync(IFormFile file, int width, int height, string folder = null, CancellationToken ct = default)
        {
            return await UploadInternalAsync
            (
                file,
                width,
                height,
                folder,
                overwrite: false,
                invalidateCdn: false,
                publicId: null,
                ct
            );
        }


        private async Task<ImageUploadResultDto> UploadInternalAsync(
            IFormFile file,
            int width,
            int height,
            string folder,
            bool overwrite,
            bool invalidateCdn,
            string publicId,
            CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is required to upload on cloudinary");

            await using var stream = file.OpenReadStream();

            var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = string.IsNullOrEmpty(publicId), // only use filename if new upload
                UniqueFilename = string.IsNullOrEmpty(publicId), // only unique name for new upload
                Overwrite = overwrite,
                Invalidate = invalidateCdn,
                PublicId = publicId,
                Transformation = new Transformation()
                    .Width(width)
                    .Height(height)
                    .Crop("fill")
                    .Gravity("auto")
            };


            var uploadResult = await cloudinary.UploadAsync(uploadParams, ct);
            if (uploadResult.Error != null)
                throw new InternalServerException(uploadResult.Error.Message);

            return MapUploadResult(uploadResult);
        }

        private ImageUploadResultDto MapUploadResult(CloudinaryDotNet.Actions.ImageUploadResult uploadResult)
        {
            return new()
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString() ?? string.Empty,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Bytes = uploadResult.Bytes
            };
        }

    }
}
