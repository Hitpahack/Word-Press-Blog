using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WP.DTOs.UserDtos;

namespace WP.Core
{
    public interface ITokenService
    {
        string GenerateToken(UserResponseDTO user);
    }
}
