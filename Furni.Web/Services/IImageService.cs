namespace Furni.Web.Services
{
    public interface IImageService
    {
        Task<(bool isUploaded, string? errorMessage)> UploadeAsynce(IFormFile image, string imageName, string folderPath, bool hasThumbnail);
        void Delete(string imagePath, string? imageThumbnailPath = null);
    }
}
