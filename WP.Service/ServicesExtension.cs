using Microsoft.Extensions.DependencyInjection;
using WP.EDTOs.Categories;
using WP.Service.Categories;
using WP.Service.Medias;
using WP.Service.Users;
using WP.Service.Yoast;

namespace WP.Service
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddWebService(this IServiceCollection service, Action<IServiceCollection> callback = null)
        {
            service.AddSingleton<IPostService, PostService>();
            service.AddSingleton<IUsersService, UsersService>();
            service.AddSingleton<ITermsService, TermsService>();
            service.AddSingleton<IMediaService, MediaService>();
            service.AddSingleton<IYoastServices, YoastServices>();
            if (callback != null)
            {
                callback.Invoke(service);
                callback.Invoke(service);
            }
            return service;
        }
        
    }

    public static class ServicesCoreExtenstion
    {
		public static List<CATEGORIES_TERMS_DTO> GetAsSingleList(List<CATEGORIES_TERMS_DTO> categories)
		{
			var flatList = new List<CATEGORIES_TERMS_DTO>();

			void Flatten(CATEGORIES_TERMS_DTO category)
			{
				flatList.Add(category); // Add the parent first
				if (category.Subcategory != null && category.Subcategory.Any())
				{
					foreach (var sub in category.Subcategory)
					{
						sub.Most_Used_Category = category.Most_Used_Category; // Preserve parent reference if needed
						Flatten(sub);
					}
				}
			}

			foreach (var category in categories)
			{
				Flatten(category);
			}

			return flatList;
		}
	}
}
