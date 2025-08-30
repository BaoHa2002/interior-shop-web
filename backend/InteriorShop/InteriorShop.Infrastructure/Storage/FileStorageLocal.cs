using InteriorShop.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace InteriorShop.Infrastructure.Storage
{
    public class FileStorageLocal : IFileStorage
    {
        private readonly string _rootPath;
        public FileStorageLocal(IHostEnvironment env)
        {
            _rootPath = Path.Combine(env.ContentRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(_rootPath);
        }

        public async Task<string> SaveAsync(Stream stream, string fileName, string contentType)
        {
            var safeName = Guid.NewGuid().ToString("N") + Path.GetExtension(fileName);
            var path = Path.Combine(_rootPath, safeName);
            using var fs = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fs);
            return "/uploads/" + safeName;
        }

        public Task DeleteAsync(string filePath)
        {
            var fullPath = Path.Combine(_rootPath, Path.GetFileName(filePath));
            if (File.Exists(fullPath)) File.Delete(fullPath);
            return Task.CompletedTask;
        }
    }
}