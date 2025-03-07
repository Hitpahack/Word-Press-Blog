using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data.Repositories;
using WP.DTOs;

namespace WP.Services
{
    public interface IMediaService
    {
        Task<ApiResponse<bool>> UploadMediaAsync(IFormFile file);
    }
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<ApiResponse<bool>> UploadMediaAsync(IFormFile file)
        {
            bool response = await _mediaRepository.UploadMediaAsync(file);
            if(!response) 
                return new FailedApiResponse<bool>("Unable to upload media");
            return new SuccessApiResponse<bool>(response,"Media uploaded sucessfully");
            
        }
    }

}
