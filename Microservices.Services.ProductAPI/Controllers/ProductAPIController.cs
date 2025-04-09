using AutoMapper;
using Microservices.Services.ProductAPI.Models.Dto;
using Microservices.Services.ProductAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microservices.Services.ProductAPI.Data;

namespace Microservices.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public ProductAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Product> productList = _db.Products.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
                _responseDto.Message = "Productos obtenidos con exito";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message += ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetById(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(x => x.ProductId == id);
                if (product != null)
                {
                    _responseDto.Result = _mapper.Map<ProductDto>(product);
                    _responseDto.Message = $"Producto {product.Name} obtenido con exito";
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Producto no encontrado";
                }
               
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message += ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto != null)
                {
                    Product product = _mapper.Map<Product>(productDto);
                    _db.Products.Add(product);
                    _db.SaveChanges();

                    _responseDto.Result = product;
                    _responseDto.Message = $"Producto {product.Name}  creado con exito";
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Ocurrio un error al intentar guardar el producto";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message += ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto != null)
                {
                    Product product = _mapper.Map<Product>(productDto);
                    _db.Products.Update(product);
                    _db.SaveChanges();

                    _responseDto.Result = product;
                    _responseDto.Message = $"Producto {product.Name}  actualizado con exito";
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Ocurrio un error al intentar actualizar el producto";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message += ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "El id del producto no es valido";
                }
                else
                {
                    Product product = _db.Products.FirstOrDefault(x => x.ProductId == id);
                    if (product != null)
                    {
                        _db.Products.Remove(product);
                        _db.SaveChanges();

                        _responseDto.Result = _mapper.Map<ProductDto>(product);
                        _responseDto.Message = $"Producto {product.Name}  eliminado con exito";

                    }
                    else
                    {
                        _responseDto.IsSuccess = false;
                        _responseDto.Message = "Producto no encontrado";
                    }

                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message += ex.Message;
            }
            return _responseDto;
        }


    }
}
