﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Furni.Web.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<(bool isUploaded, string? errorMessage)> UploadeAsynce(IFormFile image, string imageName, string folderPath, bool hasThumbnail)
        {
            var extension = Path.GetExtension(image.FileName);
            if (!_allowedExtensions.Contains(extension))
                return (isUploaded: false, errorMessage: Errors.NotAllowedExtension);

            if (image.Length > _maxAllowedSize)
                return (isUploaded: false, errorMessage: Errors.MaxSize);


            if (!Directory.Exists($"{_webHostEnvironment.WebRootPath}{folderPath}"))
                Directory.CreateDirectory($"{_webHostEnvironment.WebRootPath}{folderPath}");

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}", imageName);

            using var stream = File.Create(path);
            await image.CopyToAsync(stream);
            stream.Dispose();

            if (hasThumbnail)
            {
                if (!Directory.Exists($"{_webHostEnvironment.WebRootPath}{folderPath}/thumb"))
                    Directory.CreateDirectory($"{_webHostEnvironment.WebRootPath}{folderPath}/thumb");

                var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}/thumb", imageName);

                using var loadedImage = Image.Load(image.OpenReadStream());
                var ratio = (float)loadedImage.Width / 200;
                var height = loadedImage.Height / ratio;
                loadedImage.Mutate(i => i.Resize(width: 200, height: (int)height));
                loadedImage.Save(thumbPath);

            }

            return (isUploaded: true, errorMessage: null);
        }


        public void Delete(string imagePath, string? imageThumbnailPath = null)
        {
            var oldImagePath = $"{_webHostEnvironment.WebRootPath}{imagePath}";

            if (File.Exists(oldImagePath))
                File.Delete(oldImagePath);

            if (!string.IsNullOrEmpty(imageThumbnailPath))
            {
                var oldThumbPath = $"{_webHostEnvironment.WebRootPath}{imageThumbnailPath}";
                if (File.Exists(oldThumbPath))
                    File.Delete(oldThumbPath);
            }
        }
    }
}
