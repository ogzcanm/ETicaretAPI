using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETicaretAPI.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }
        [HttpGet]
        public async Task Get()
        {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //    new()
            //    {
            //        Id = Guid.NewGuid(), Name = "Product 1", Price = 100, CreateDate = DateTime.UtcNow, Stock=10
            //    },
            //    new()
            //    {
            //        Id = Guid.NewGuid(), Name = "Product 2", Price = 200, CreateDate = DateTime.UtcNow, Stock=3
            //    },
            //    new()
            //    {
            //        Id = Guid.NewGuid(), Name = "Product 3", Price = 300, CreateDate = DateTime.UtcNow, Stock=30
            //    },
            //});
            //var count = await _productWriteRepository.SaveAsync();

            Product p = await _productReadRepository.GetByIdAsync("9fc55652-d890-4d2c-90cc-2c9a0d5ee668",false);
            p.Name = "Mehmet";
            await _productWriteRepository.SaveAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }
    }
}
