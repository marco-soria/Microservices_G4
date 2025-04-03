using AutoMapper;
using Microservices.Services.CouponAPI.Data;
using Microservices.Services.CouponAPI.Models;
using Microservices.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly IMapper _mapper;

        public CouponAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Coupon> couponList = _db.Coupon.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
                _response.Message = "Coupons retrieved successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "An error ocurred when retrieving the lsit of coupons " + ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetById(int id)
        {
            try
            {
                Coupon coupon = _db.Coupon.FirstOrDefault(x => x.CouponId == id);
                if (coupon is not null)
                {
                    var couponDto = _mapper.Map<CouponDto>(coupon);
                    _response.Result = couponDto;
                    _response.Message = $"Coupon {couponDto.CouponCode} retrieved successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon not found";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "An error ocurred when retrieving the coupon " + ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _db.Coupon.FirstOrDefault(x => x.CouponCode.ToLower().Trim() == code.ToLower().Trim());
                if (coupon is not null)
                {
                    _response.Result = _mapper.Map<CouponDto>(coupon);
                    _response.Message = $"Coupon {coupon.CouponCode} retrieved successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon not found";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "An error ocurred when retrieving the coupon " + ex.Message;
            }

            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                if (couponDto is not null)
                {
                    Coupon coupon = _mapper.Map<Coupon>(couponDto);
                    _db.Coupon.Add(coupon);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<CouponDto>(coupon);
                    _response.Message = $"Coupon {couponDto.CouponCode} successfully created";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon {couponDto.CouponCode} is not valid";
                }

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = "An error ocurred when creating the coupon " + ex.Message;

            }
            return _response;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                if (couponDto is not null)
                {
                    Coupon coupon = _mapper.Map<Coupon>(couponDto);
                    _db.Coupon.Update(coupon);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<CouponDto>(coupon);
                    _response.Message = $"Coupon {couponDto.CouponCode} successfully updated";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Coupon {couponDto.CouponCode} is not valid";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "An error ocurred when updating the coupon " + ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        public ResponseDto Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon id is not valid";
                }
                else
                {
                    Coupon coupon = _db.Coupon.FirstOrDefault(x => x.CouponId == id);
                    if (coupon != null)
                    {
                        _db.Coupon.Remove(coupon);
                        _db.SaveChanges();

                        _response.Result = _mapper.Map<CouponDto>(coupon);
                        _response.Message = $"Coupon {coupon.CouponCode} deleted successfully";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = $"The coupon entered is not valid";
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "An error ocurred when deleting the coupon " + ex.Message;
            }

            return _response;
        }
    }
}
