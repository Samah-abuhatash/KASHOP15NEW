using KaSHop.DAL.DTO.Requet;
using KaSHop.DAL.DTO.Responses;
using KaSHop.DAL.models;
using KaSHop.DAL.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.BLL
{
    public class ProductService : IProductService
    {
        private readonly IFileService _fileService;
        private readonly IGenericRepository<Product> _productRepo;

        public ProductService(
            IFileService fileService,
            IGenericRepository<Product> productRepo)
        {
            _fileService = fileService;
            _productRepo = productRepo;
        }

        public async Task<bool> CreateAsync(
            ProductRequest request,
            CancellationToken cancellationToken = default)
        {
            var ImageName = await _fileService.UploadAsync(
                request.MainImage, cancellationToken);

            if (ImageName is null)
                return false;

            var product = request.Adapt<Product>();

            product.MainImage = ImageName;

            try
            {
                await _productRepo.CreateAsync(product, cancellationToken);
            }
            catch
            {
                _fileService.Delete(ImageName);
                throw;
            }

            return true;
        }
        public async Task<List<ProductResponse>> GetAllAsync(
    CancellationToken cancellationToken = default)
        {
            var products = await _productRepo.GetAllAsync(
                includes: [p => p.Translations, p => p.Category.Translations],
                cancellationToken: cancellationToken
            );

            return products.Adapt<List<ProductResponse>>();
        }
    }
   
}

