using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;

namespace Microservices.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        //POST
        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
           return await _baseService.SendAsync(new RequestDto()
           {
               ApiType = SD.ApiType.POST,
               Url = SD.CouponAPIBase + "/api/CouponAPI",
               Data = couponDto
           });
        }
        //DELETE
        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/CouponAPI/" + id
            });
        }
        //GETALL
        public async Task<ResponseDto> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/CouponAPI"
            });
        }

        //GET COUPON BY CODE
        public async Task<ResponseDto> GetCouponByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/CouponAPI/GetByCode/" + couponCode
            });
        }
        //GET COUPON BY ID
        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/CouponAPI/" + id
            });
        }

        //PUT
        public Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/CouponAPI"
            });
        }
    }
}
