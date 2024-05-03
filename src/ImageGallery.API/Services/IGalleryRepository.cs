using ImageGallery.API.Entities;
using System.Linq.Expressions;

namespace ImageGallery.API.Services
{
    public interface IGalleryRepository
    {
        Task<IEnumerable<Image>> GetImagesAsync(string? ownerID = null , Expression<Func<Image , bool>> filter = null);
        Task<bool> IsImageOwnerAsync(Guid id, string ownerId);
        Task<Image?> GetImageAsync(Guid id);
        Task<bool> ImageExistsAsync(Guid id);
        void AddImage(Image image);
        void UpdateImage(Image image);
        void DeleteImage(Image image);
        Task<bool> SaveChangesAsync();
    }
}
