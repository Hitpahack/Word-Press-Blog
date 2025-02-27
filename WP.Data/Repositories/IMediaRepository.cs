using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public interface IMediaRepository
    {
        Task<bool> UploadMediaAsync(IFormFile file);
    }
}
