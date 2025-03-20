using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Drawing;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Medias;
using WP.Repository;
using WP.Common;

namespace WP.Service.Medias
{

    public interface IMediaService : IDisposable
    {
        Task<ResponseDto<string>> Add_FeaturedImage(IFormFile file, WP_POST_MEDIA_ADD reqDto, ulong postid);
        Task<ResponseDto<bool>> Remove_FeaturedImage(ulong postid);
    }
    public class MediaService : BaseServices, IMediaService
    {
        #region private
        private readonly IRepository<WpPost> _repoPost;
        private readonly IRepository<WpPostmetum> _repoPostMeta;
        //private readonly IRepository<GET_POSTS_PAGED_SP> _get_posts_paged_sp;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public MediaService(IRepository<WpPost> repoPost,
           IRepository<WpPostmetum> repoPostMeta,
            IMapper mapper)
        {
            //_get_posts_paged_sp = get_posts_paged_sp;
            _repoPost = repoPost;
            _repoPostMeta = repoPostMeta;
            _mapper = mapper;
        }
        #endregion

        public async Task<ResponseDto<string>> Add_FeaturedImage(IFormFile file, WP_POST_MEDIA_ADD reqDto, ulong postid)
        {
            if (file?.Length > 0)
            {
                (int width, int height)[] cropSizes = {
                    (150, 150),  // Thumbnail
                    (300, 200),  // Small
                    (1024, 768),  // Medium
                    (1200, 800)  // Large
                };

                string filepathsave = _appSetting.MediaSetting.FileSavePath;
                filepathsave = filepathsave
                    .Replace("{year}", DateTime.Now.Year.ToString())
                    .Replace("{month}", DateTime.Now.Month.ToString("D2"));

                string uploadFolder = string.Concat(_environment.WebRootPath, filepathsave);
                // Ensure the directory exists
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                string uniqueFileName = file.FileName.Replace(" ", "-").Trim().ToLower();
                string uniqueFileName_withouext = uniqueFileName.Split('.')[0];
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                string fullfileurl = string.Concat(_appSetting.MediaSetting.BasePath, filepathsave, uniqueFileName);
                // Save file to folder
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                foreach (var size in cropSizes)
                {
                    string outputPath = string.Concat(uploadFolder, uniqueFileName_withouext, $"-{size.width}x{size.height}.jpg");
                    CropAndSaveImage(file, outputPath, size.width, size.height);
                }

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out string contentType))
                {
                    contentType = "application/octet-stream"; // Default if unknown
                }
                string mimeType = contentType;

                WpPost wppost = _mapper.Map<WpPost>(reqDto);
                wppost.PostContent = "attachment";
                wppost.PostStatus = "inherit";
                wppost.PostMimeType = mimeType;
                wppost.PingStatus = "closed";
                wppost.PostType = "attachment";
                wppost.Guid = fullfileurl;
                wppost.PostParent = postid;

                await _repoPost.InsertAsync(wppost);
               
                await _repoPostMeta.InsertAsync(new WpPostmetum
                {
                    PostId = postid,
                    MetaKey = "_thumbnail_id",
                    //MetaKey = "_wp_attached_file",
                    //MetaValue = $"{DateTime.UtcNow:yyyy/MM}/{uniqueFileName}"
                    MetaValue = wppost.Id.ToString()
                });
                object metadata = new { file = $"{DateTime.UtcNow:yyyy/MM}/{uniqueFileName}" };

                if (mimeType.StartsWith("image"))
                {
                    metadata = new
                    {
                        width = 1920,
                        height = 1080,
                        file = $"{DateTime.UtcNow:yyyy/MM}/{uniqueFileName}",
                        sizes = new
                        {
                            thumbnail = new { file = $"{Path.GetFileNameWithoutExtension(uniqueFileName)}-150x150.jpg", width = 150, height = 150 },
                            medium = new { file = $"{Path.GetFileNameWithoutExtension(uniqueFileName)}-300x200.jpg", width = 300, height = 200 },
                            large = new { file = $"{Path.GetFileNameWithoutExtension(uniqueFileName)}-1024x768.jpg", width = 1024, height = 768 }
                        }
                    };
                }
                else if (mimeType.StartsWith("video"))
                {
                    metadata = new
                    {
                        duration = "120",
                        resolution = "1920x1080",
                        file = $"{DateTime.UtcNow:yyyy/MM}/{uniqueFileName}"
                    };
                }
                else if (mimeType == "application/pdf")
                {
                    metadata = new
                    {
                        page_count = "5",
                        file_size = "2MB",
                        file = $"{DateTime.UtcNow:yyyy/MM}/{uniqueFileName}"
                    };
                }
                else if (mimeType.Contains("spreadsheet") || mimeType.Contains("excel"))
                {
                    metadata = new
                    {
                        sheet_count = "3",
                        file_size = "1.5MB",
                        file = $"{DateTime.UtcNow:yyyy/MM}/{uniqueFileName}"
                    };
                }

                await _repoPostMeta.InsertAsync(new WpPostmetum
                {
                    PostId = wppost.Id,
                    MetaKey = "_wp_attachment_metadata",
                    MetaValue = JsonConvert.SerializeObject(metadata)
                });
                
                return new SuccessResponseDto<string>(fullfileurl);
            }
            return new SuccessResponseDto<string>("");

        }

        public async Task<ResponseDto<bool>> Remove_FeaturedImage(ulong postid)
        {
            try
            {
                if (postid > 0)
                {
                    var img = _repoPost.GetFirstOrDefault(s => s.PostParent == postid && s.PostType == "attachment");
                    _repoPost.Delete(img);

                    var imgMeta = _repoPostMeta.GetAll(s => s.PostId == postid && (s.MetaKey == "_thumbnail_id" || s.MetaKey == "_wp_attachment_metadata")).ToList();
                    foreach (var item in imgMeta)
                    {
                        _repoPostMeta.Delete(item);
                    }

                    return new SuccessResponseDto<bool>(true);
                }
                return new FailedResponseDto<bool>("postid required");
            }
            catch (Exception ex)
            {

                return new FailedResponseDto<bool>(ex.GetActualError());
            }
        }
        void CropAndSaveImage(IFormFile file, string outputPath, int width, int height)
        {
            using (Image originalImage = Image.FromStream(file.OpenReadStream()))
            {
                using (Bitmap croppedImage = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(croppedImage))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        // Crop the center of the image
                        int cropX = (originalImage.Width - width) / 2;
                        int cropY = (originalImage.Height - height) / 2;

                        Rectangle sourceRectangle = new Rectangle(cropX, cropY, width, height);
                        Rectangle destinationRectangle = new Rectangle(0, 0, width, height);

                        graphics.DrawImage(originalImage, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
                    }

                    croppedImage.Save(outputPath, ImageFormat.Jpeg);
                }
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoPost.Dispose();
            _repoPostMeta.Dispose();
        }
    }
}
