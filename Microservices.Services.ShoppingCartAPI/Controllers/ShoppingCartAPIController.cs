using AutoMapper;
using Microservices.Services.ShopingCartAPI.Data;
using Microservices.Services.ShoppingCartAPI.Models;
using Microservices.Services.ShoppingCartAPI.Models.Dto;
using Microservices.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microservices.Services.ShoppingCartAPI.Controllers
{
    [Route("api/shoppingCart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IConfiguration _configuration;

        public ShoppingCartAPIController(ApplicationDbContext db,
            IMapper mapper,
            IProductService productService,
            ICouponService couponService,
            IConfiguration configuration)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _configuration = configuration;
        }


        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(x => x.UserId == cartDto.CartHeaderDto.UserId);
                cartFromDb.CouponCode = cartDto.CartHeaderDto.CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _responseDto.Result = true;

            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders.FirstAsync(x => x.UserId == cartDto.CartHeaderDto.UserId);
                cartFromDb.CouponCode = "";
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeaderDto.UserId);

                if (cartHeaderFromDb == null)
                {
                    //si no exites la cabecera -> la creamos
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeaderDto);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    //Relacionamos los details de cada producto con su cabezera
                    cartDto.CartDetailsDto.First().CartHeaderId = cartHeader.CartHeaderId;
                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetailsDto);
                    //CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetailsDto.First());
                    _db.CartDetails.Add(cartDetails);
                    await _db.SaveChangesAsync();

                    _responseDto.Result = cartDto;

                }
                else
                {
                    //si encuentra la cabezera
                    //revisamos si los details tienen los mismos productos
                    var carDetailsFromDb = await _db.CartDetails.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetailsDto.First().ProductId &&
                        x.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (carDetailsFromDb == null)
                    {
                        //crear los CartDetails
                        cartDto.CartDetailsDto.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetailsDto.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //actualizar los campos de los detalles
                        cartDto.CartDetailsDto.First().Count += carDetailsFromDb.Count;
                        cartDto.CartDetailsDto.First().CartHeaderId = carDetailsFromDb.CartHeaderId;
                        cartDto.CartDetailsDto.First().CartDetailsId = carDetailsFromDb.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetailsDto.First()));
                        await _db.SaveChangesAsync();
                    }

                    _responseDto.Result = cartDto;

                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }


        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = new CartDto()
                {
                    CartHeaderDto = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(x => x.UserId == userId)),
                };

                cartDto.CartDetailsDto = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails
                    .Where(x => x.CartHeaderId == cartDto.CartHeaderDto.CartHeaderId));

                //IProductService
                IEnumerable<ProductDto> productDtoList = await _productService.GetProducts();
                foreach (var item in cartDto.CartDetailsDto)
                {
                    item.ProductDto = productDtoList.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cartDto.CartHeaderDto.CartTotal += (item.ProductDto.Price * item.Count);
                }

                //ICouponService - valida si existe el coupon
                if (!string.IsNullOrEmpty(cartDto.CartHeaderDto.CouponCode))
                {
                    CouponDto couponDto = await _couponService.GetCoupon(cartDto.CartHeaderDto.CouponCode);
                    if (couponDto != null && cartDto.CartHeaderDto.CartTotal > couponDto.MinAmount)
                    {
                        cartDto.CartHeaderDto.CartTotal -= couponDto.DiscountAmount;
                        cartDto.CartHeaderDto.Discount = couponDto.DiscountAmount;
                    }
                }

                _responseDto.Result = cartDto;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message.ToString();
            }

            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(x => x.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = _db.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var carHeaderFromDb = await _db.CartHeaders.FirstAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(carHeaderFromDb);
                }

                await _db.SaveChangesAsync();
                _responseDto.Result = true;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message.ToString();
            }

            return _responseDto;
        }

    }


}
