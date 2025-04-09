using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? listProductDtos = new List<ProductDto>();

            ResponseDto? responseDto = await _productService.GetAllProductAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                listProductDtos = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return View(listProductDtos);
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _productService.CreateProductAsync(productDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Producto creado exitosamente";
                    RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto.Message;
                }
            }
            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if (responseDto != null & responseDto.IsSuccess)
            {
                ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(productDto);
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _productService.UpdateProductAsync(productDto);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Producto actualizado exitosamente";
                    RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }
            return View(productDto);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            //ProductDto? model = new();
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }
    }
}
