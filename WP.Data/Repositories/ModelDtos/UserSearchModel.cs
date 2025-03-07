using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public partial class UserSearchDto
    {
        public UserSearchDto()
        {
                
        }
        public WpPost Post { get; set; }
        public WpUser User { get; set; }
        public WpUsermetum Usermetum { get; set; }
        


    }

    public partial class UserResponseDto
    {
        public UserResponseDto()
        {

        }
       
        


    }
}
