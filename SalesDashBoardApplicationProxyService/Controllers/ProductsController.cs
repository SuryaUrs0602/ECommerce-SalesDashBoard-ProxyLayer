using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesDashBoardApplicationProxyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SalesDashBoardClient _salesDashBoardClient;
        private readonly ILogger<ProductsController> _logger;


        public ProductsController(SalesDashBoardClient salesDashBoardClient, ILogger<ProductsController> logger)
        {
            _salesDashBoardClient = salesDashBoardClient;
            _logger = logger;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            try
            {
                _logger.LogInformation("Creating a new Product");
                await _salesDashBoardClient.ProductsPOSTAsync(productDto);
                return Created();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while creating a new product");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Deleting a product");
                await _salesDashBoardClient.ProductsDELETEAsync(id);
                return NoContent();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while deleting a product");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [HttpGet]
        public async Task<IActionResult> AllProductsData()
        {
            try
            {
                _logger.LogInformation("Fetching all products data");
                var products = await _salesDashBoardClient.ProductsAllAsync();
                return Ok(products);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while fetching products data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> ProductDataById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching product data by id");
                var product = await _salesDashBoardClient.ProductsGETAsync(id);
                return Ok(product);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while fetching products data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [HttpGet("category/{category}")]
        public async Task<IActionResult> ProductDataByCategory(string category)
        {
            try
            {
                _logger.LogInformation("Fetching products data by category");
                var products = await _salesDashBoardClient.CategoryAsync(category);
                return Ok(products);
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while fetching products data");
                return StatusCode(500, new { error = "Could not process due to some error" });
            }
        }



        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProductDetails(int id, JsonPatchDocument<ProductDto> patchDocument)
        {
            try
            {
                _logger.LogInformation("Updating the product details by product id");
                var operations = patchDocument.Operations
                    .Select(op => new SalesDashBoardApplication.Operation
                    {
                        Path = op.path,
                        Op = op.op,
                        From = op.from,
                        Value = op.value
                    }).ToList();
                await _salesDashBoardClient.ProductsPATCHAsync(id, operations);
                return NoContent();
            }

            catch (ApiException ex)
            {
                _logger.LogError(ex, "Error occured while updating products data");
                return StatusCode(500, new { error = "Could not process due to some error" });      
            }
        }
    }
}
