using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DataContext;

namespace WP.Service.Users
{
    public class BindModal
    {
        public WpPost Post { get; set; }
        public WpUser User { get; set; }
        public WpUsermetum Usermetum { get; set; }
    }
   
}
