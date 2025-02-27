using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;


namespace WP.Core
{
    public interface ITokenService
    {
        string GenerateToken(UserResponseDTO user);
    }
}
