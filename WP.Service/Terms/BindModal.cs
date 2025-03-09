using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DataContext;

namespace WP.Service.Terms
{
    public class BindModal
    {
        public WpPost Post { get; set; }
        public WpUser WpUser { get; set; }
        public WpTerm Catregories { get; set; }
        public WpTerm Tags { get; set; }
    }
   
}
