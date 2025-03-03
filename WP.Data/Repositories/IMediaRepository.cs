using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public interface IMediaRepository
    {
        Task<bool> UploadMediaAsync(IFormFile file);
    }
    public class MediaRepository : IMediaRepository
    {
        private readonly BlogContext _dbContext;
        private readonly string _storagePath;

        public MediaRepository(BlogContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _storagePath = configuration["MediaSettings:StoragePath"] ?? "uploads";

            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public async Task<bool> UploadMediaAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(_storagePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = "application/octet-stream"; // Default if unknown
            }
            string mimeType = contentType;


            var post = new WpPost
            {
                PostTitle = fileName,
                PostMimeType = mimeType,
                PostContent = "",
                PostDate = DateTime.Now,
                PostDateGmt = DateTime.UtcNow,
                PostStatus = "inherit",
                PingStatus = "closed",
                ToPing = "",
                PostPassword = "",
                PostType = "attachment",
                PostName = fileName,
                Guid = filePath,
                MenuOrder = 0,
                CommentCount = 0,
                Pinged = "",
                PostContentFiltered = "",
                PostExcerpt = "This is an image caption.",
            };

            _dbContext.WpPosts.Add(post);
            await _dbContext.SaveChangesAsync();

            await _dbContext.WpPostmeta.AddAsync(new WpPostmetum
            {
                PostId = post.Id,
                MetaKey = "_wp_attached_file",
                MetaValue = $"{DateTime.UtcNow:yyyy/MM}/{fileName}"
            });
            object metadata = new { file = $"{DateTime.UtcNow:yyyy/MM}/{fileName}" };

            if (mimeType.StartsWith("image"))
            {
                metadata = new
                {
                    width = 1920,
                    height = 1080,
                    file = $"{DateTime.UtcNow:yyyy/MM}/{fileName}",
                    sizes = new
                    {
                        thumbnail = new { file = $"{Path.GetFileNameWithoutExtension(fileName)}-150x150.jpg", width = 150, height = 150 },
                        medium = new { file = $"{Path.GetFileNameWithoutExtension(fileName)}-300x200.jpg", width = 300, height = 200 },
                        large = new { file = $"{Path.GetFileNameWithoutExtension(fileName)}-1024x768.jpg", width = 1024, height = 768 }
                    }
                };
            }
            else if (mimeType.StartsWith("video"))
            {
                metadata = new
                {
                    duration = "120",
                    resolution = "1920x1080",
                    file = $"{DateTime.UtcNow:yyyy/MM}/{fileName}"
                };
            }
            else if (mimeType == "application/pdf")
            {
                metadata = new
                {
                    page_count = "5",
                    file_size = "2MB",
                    file = $"{DateTime.UtcNow:yyyy/MM}/{fileName}"
                };
            }
            else if (mimeType.Contains("spreadsheet") || mimeType.Contains("excel"))
            {
                metadata = new
                {
                    sheet_count = "3",
                    file_size = "1.5MB",
                    file = $"{DateTime.UtcNow:yyyy/MM}/{fileName}"
                };
            }

            await _dbContext.WpPostmeta.AddAsync(new WpPostmetum
            {
                PostId = post.Id,
                MetaKey = "_wp_attachment_metadata",
                MetaValue = JsonSerializer.Serialize(metadata)
            });
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }

}
