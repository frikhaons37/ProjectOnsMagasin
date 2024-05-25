using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectOnsMagasin.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductApiController(IProductRepository repository, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _repository.GetTheBestSellingItem());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await _repository.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProductRequest request)
        {
            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                Brand = request.Brand,
                Price = request.Price,
                Qunatity = request.Qunatity,
                CategoryId = request.CategoryId,
            };

            if (request.Image != null)
            {
                var uploadsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "products");

                if (!Directory.Exists(uploadsDirectory))
                    Directory.CreateDirectory(uploadsDirectory);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
                var filePath = Path.Combine(uploadsDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.Image.CopyTo(stream);
                }

                product.ImagePath = $"~/Products/{fileName}";
            }
            await _repository.Add(product);
            return Ok();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id ,[FromForm] ProductRequestForUpdate request )
        {
            Product product = new()
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                Brand = request.Brand,
                Price = request.Price,
                Qunatity = request.Qunatity,
                CategoryId = request.CategoryId,
            };

            if (request.Image != null)
            {
                var uploadsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "products");

                if (!Directory.Exists(uploadsDirectory))
                    Directory.CreateDirectory(uploadsDirectory);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
                var filePath = Path.Combine(uploadsDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.Image.CopyTo(stream);
                }

                product.ImagePath = $"~/Products/{fileName}";
            }
                await _repository.Edit(product);
                return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Remove(id);
            return Ok();
        }
    }
}
