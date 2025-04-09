using AutoMapper;
using Microservices.Services.CouponAPI.Data;
using Microservices.Services.CouponAPI.Models;
using Microservices.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "ADMIN")]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Coupon> couponList = _db.Coupon.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
                _response.Message = "Cupones obtenidos con exito.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Ocurrio un error al obtener los cupones " + ex.Message;
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
                if (coupon != null)
                {
                    var couponDto = _mapper.Map<CouponDto>(coupon);
                    _response.Result = couponDto;
                    _response.Message = $"Cupon {couponDto.CouponCode} recuperado con exito";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Cupon no encontrado";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Ocurrio un error al intentar obtener el cupon " + ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon =
                    _db.Coupon.FirstOrDefault(x => x.CouponCode.ToLower().Trim() == code.ToLower().Trim());
                if (coupon != null)
                {
                    _response.Result = _mapper.Map<CouponDto>(coupon);
                    _response.Message = $"Cupon {coupon.CouponCode} recuperado con exito";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Cupon no encontrado";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Ocurrio un error al intentar obtener el cupon " + ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                if (couponDto != null)
                {
                    Coupon coupon = _mapper.Map<Coupon>(couponDto);
                    _db.Coupon.Add(coupon);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<CouponDto>(coupon);
                    _response.Message = $"Cupon {couponDto.CouponCode} creado con exito";

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Cupon {couponDto.CouponCode} ingresado no es valido";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Ocurrio un error al crear el cupon " + ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                if (couponDto != null)
                {
                    Coupon coupon = _mapper.Map<Coupon>(couponDto);

                    _db.Coupon.Update(coupon);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<CouponDto>(coupon);
                    _response.Message = $"Cupon {couponDto.CouponCode} actualizado con exito";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Cupon {couponDto.CouponCode} ingresado no es valido";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Ocurrio un error al crear el cupon " + ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "El id del cupon no es valido";
                }
                else
                {
                    Coupon coupon = _db.Coupon.FirstOrDefault(x => x.CouponId == id);
                    if (coupon != null)
                    {
                        _db.Coupon.Remove(coupon);
                        _db.SaveChanges();

                        _response.Result = _mapper.Map<CouponDto>(coupon);
                        _response.Message = $"Cupon {coupon.CouponCode} eliminado con exito";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = $"Cupon ingresado no es valido";
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Ocurrio un error al crear el cupon " + ex.Message;
            }

            return _response;
        }

    }
}
