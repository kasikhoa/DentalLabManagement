using DentalLabManagement.BusinessTier.Payload.NewFolder;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductResponse> CreateProduct(ProductRequest productRequest);
    }
}
