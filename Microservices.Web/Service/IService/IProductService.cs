﻿using Microservices.Web.Models;

namespace Microservices.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProductAsync();
        Task<ResponseDto> GetProductByIdAsync(int id);
        Task<ResponseDto> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto> DeleteProductAsync(int id);
    }
}
