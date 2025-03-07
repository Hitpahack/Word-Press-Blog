using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Web
{
    public class StaticData
    {
        public static List<SelectListItem> GetRoles
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "wpseo_editor", Text = "SEO Editor" },
                    new SelectListItem { Value = "wpseo_manager", Text = "SEO Manager" },
                    new SelectListItem { Value = "subscriber", Text = "Subscriber", Selected = true }, // Default selected
                    new SelectListItem { Value = "contributor", Text = "Contributor" },
                    new SelectListItem { Value = "author", Text = "Author" },
                    new SelectListItem { Value = "editor", Text = "Editor" },
                    new SelectListItem { Value = "administrator", Text = "Administrator" }
                };
            }

        }

        public static List<SelectListItem> GetRolesSelected(string selectedrole)
        {
            var items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "wpseo_editor", Text = "SEO Editor" },
                    new SelectListItem { Value = "wpseo_manager", Text = "SEO Manager" },
                    new SelectListItem { Value = "subscriber", Text = "Subscriber", Selected = true }, // Default selected
                    new SelectListItem { Value = "contributor", Text = "Contributor" },
                    new SelectListItem { Value = "author", Text = "Author" },
                    new SelectListItem { Value = "editor", Text = "Editor" },
                    new SelectListItem { Value = "administrator", Text = "Administrator" }
                };
            var selecteitem = items.FirstOrDefault(s => s.Value == selectedrole);
            selecteitem.Selected = true;
            return items;
        }
    }
}
