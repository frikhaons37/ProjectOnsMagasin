using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace ProjectOnsMagasin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly HttpClient _httpClient;

        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment hostingEnvironment,
            HttpClient httpClient)
        {

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var protocol = Request.IsHttps ? "https://" : "http://";
            var host = Request.Host.Host;
            var port = Request.Host.Port;
            var url = $"{protocol}{host}:{port}/api/product/";

            IEnumerable<Product> products = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(url)
                ?? new List<Product>();

            return View(products);
        }
        public async Task<IActionResult> ListProduct()
        {
            var protocol = Request.IsHttps ? "https://" : "http://";
            var host = Request.Host.Host;
            var port = Request.Host.Port;
            var url = $"{protocol}{host}:{port}/api/product/";

            IEnumerable<Product> products = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(url) 
                ?? new List<Product>();

            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var protocol = Request.IsHttps ? "https://" : "http://";
            var host = Request.Host.Host;
            var port = Request.Host.Port;
            var url = $"{protocol}{host}:{port}/api/product/{id}";
            Product? product = await _httpClient.GetFromJsonAsync<Product>(url);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductRequest request)
        {
            try
            {
                using (var formData = new MultipartFormDataContent())
                {

                    formData.Add(new StringContent(request.Name), nameof(request.Name));
                    formData.Add(new StringContent(request.Description), nameof(request.Description));
                    formData.Add(new StringContent(request.Brand), nameof(request.Brand));
                    formData.Add(new StringContent(request.Price.ToString()), nameof(request.Price));
                    formData.Add(new StringContent(request.Qunatity.ToString()), nameof(request.Qunatity));
                    formData.Add(new StringContent(request.CategoryId.ToString()), nameof(request.CategoryId));


                    var imageContent = new StreamContent(request.Image.OpenReadStream());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(request.Image.ContentType);
                    formData.Add(imageContent, nameof(request.Image), request.Image.FileName);
                    var protocol = Request.IsHttps ? "https://" : "http://";
                    var host = Request.Host.Host;
                    var port = Request.Host.Port;
                    var url = $"{protocol}{host}:{port}/api/product/";
                    await _httpClient.PostAsync(url, formData);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return BadRequest();
            }

            
        }

        // GET: ProductController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Product? product = await _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            IEnumerable<Category> categories = await _categoryRepository.GetAll();
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImagePath);
            imagePath = imagePath.Replace("~/", "");
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            MemoryStream stream = new MemoryStream(fileBytes);
            IFormFile image = new FormFile(stream, 0, fileBytes.Length, "file", Path.GetFileName(product.ImagePath));

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            ProductRequestForUpdate request = new()
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                Qunatity = product.Qunatity,
                Image = image
            };

            return View(request);
        }

        // POST: ProductController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductRequestForUpdate request)
        {
            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(request.Id.ToString()), nameof(request.Id));
                    formData.Add(new StringContent(request.Name), nameof(request.Name));
                    formData.Add(new StringContent(request.Description), nameof(request.Description));
                    formData.Add(new StringContent(request.Brand), nameof(request.Brand));
                    formData.Add(new StringContent(request.Price.ToString()), nameof(request.Price));
                    formData.Add(new StringContent(request.Qunatity.ToString()), nameof(request.Qunatity));
                    formData.Add(new StringContent(request.CategoryId.ToString()), nameof(request.CategoryId));

                    var protocol = Request.IsHttps ? "https://" : "http://";
                    var host = Request.Host.Host;
                    var port = Request.Host.Port;
                    var url = $"{protocol}{host}:{port}/api/product/{id}";

                    var imageContent = new StreamContent(request.Image.OpenReadStream());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(request.Image.ContentType);
                    formData.Add(imageContent, nameof(request.Image), request.Image.FileName);

                    await _httpClient.PutAsync(url, formData);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET: ProductController/Delete/5
        /*        public async Task<IActionResult> Delete(int id)
                {
                    Product? product = await _productRepository.GetById(id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }*/

        // POST: ProductController/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var protocol = Request.IsHttps ? "https://" : "http://";
                var host = Request.Host.Host;
                var port = Request.Host.Port;
                var url = $"{protocol}{host}:{port}/api/product/{id}";

                await _httpClient.DeleteAsync(url);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search()

        {
            string? name = HttpContext.Request.Form["name"];
            string? brand = HttpContext.Request.Form["brand"];
            int categoryId = 0;
            int.TryParse(HttpContext.Request.Form["brand"].ToString(), out categoryId);

            var result = await _productRepository.Filter(name, brand, categoryId);

            return View("Index", result);
        }

    }
}
