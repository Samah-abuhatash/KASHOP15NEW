
using KaSHop.BLL;
using KaSHop.BLL.Settings;
using KaSHop.DAL.Data;
using KaSHop.DAL.models;
using KaSHop.DAL.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace KaSHop.pl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            TEST tEST = new TEST();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            //Category

            const string defaultCulture = "en-GB";

            var supportedCultures = new[]
            {
    new CultureInfo(defaultCulture),
    new CultureInfo("es")
};

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(SupportedLanguages.Default);
                options.AddSupportedCultures(SupportedLanguages.Languages);

                options.AddSupportedUICultures(SupportedLanguages.Languages);
                options.RequestCultureProviders = new IRequestCultureProvider[]
{
    new AcceptLanguageHeaderRequestCultureProvider()
};
            });
            builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();

            builder.Services.AddScoped<IGenericRepository<Brands>, GenericRepository<Brands>>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        "Data Source=.;Database=kashop15;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
    )
);
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<
    IGenericRepository<Product>,
    GenericRepository<Product>>();
            var app = builder.Build();

            MapsterConfig.RegisterMappings();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
);


            app.MapControllers();

            app.Run();
        }
    }
}
