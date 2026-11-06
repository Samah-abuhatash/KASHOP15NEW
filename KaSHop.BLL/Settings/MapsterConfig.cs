using KaSHop.DAL.DTO.Requet;
using KaSHop.DAL.DTO.Responses;
using KaSHop.DAL.models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.BLL.Settings
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {//view name => بناء على لغه مرسله
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(
                    dest => dest.Name,
                    src => src.Translations
                        .Where(t => t.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                        .Select(t => t.Name).FirstOrDefault()
                );

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
    .Map(
        dest => dest.Name,
        src => src.Translations
            .Where(t => t.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            .Select(t => t.Name).FirstOrDefault()
    )
    .Map(
        dest => dest.Description,
        src => src.Translations
            .Where(t => t.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            .Select(t => t.Description).FirstOrDefault()
    )
    .Map(
    dest => dest.MainImageUrl,
    src => $"/images/{src.MainImage}"
).Map(
    dest => dest.CategoryName,
    src => src.Category.Translations
        .Where(t => t.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
        .Select(t => t.Name)
        .FirstOrDefault()
);
            TypeAdapterConfig<ProductRequest, Product>.NewConfig()
    .Ignore(dest => dest.MainImage);
        }
    }
}
