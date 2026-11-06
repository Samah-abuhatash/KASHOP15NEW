using KaSHop.DAL.DTO.Requet;
using KaSHop.DAL.DTO.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KaSHop.BLL
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<CategoryResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> CreateAsync(
            CategoryRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            int Id,
            CategoryRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int Id,
            CancellationToken cancellationToken = default);
    }
}