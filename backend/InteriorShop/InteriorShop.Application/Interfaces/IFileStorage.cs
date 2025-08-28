namespace InteriorShop.Application.Interfaces
{
    public interface IFileStorage
    {
        /// <summary>
        /// Lưu file và trả về đường dẫn có thể truy cập từ FE
        /// </summary>
        Task<string> SaveAsync(Stream stream, string fileName, string contentType);

        /// <summary>
        /// Xóa file theo đường dẫn tương đối
        /// </summary>
        Task DeleteAsync(string filePath);
    }
}