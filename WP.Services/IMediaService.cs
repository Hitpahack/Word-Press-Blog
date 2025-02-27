using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data.Repositories;

namespace WP.Services
{
    public interface IMediaService
    {
        Task<bool> UploadMediaAsync(IFormFile file);
    }
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<bool> UploadMediaAsync(IFormFile file)
        {
            var response = await _mediaRepository.UploadMediaAsync(file);
            return response;
        }
    }

}
