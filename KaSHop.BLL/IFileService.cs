using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaSHop.BLL
{
    public interface IFileService
    {
        Task<string?> UploadAsync(
            IFormFile file,
            CancellationToken cancellationToken = default);

        void Delete(string fileName);
    }
}
