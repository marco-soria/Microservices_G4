using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet]
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = new List<CouponDto>();

            ResponseDto? responseDto = await _couponService.GetAllCouponAsync();

            if (responseDto != null && responseDto.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
            else
            { 
                TempData["error"] = responseDto?.Message;
            }
            return View(list);
        }
    }
}
