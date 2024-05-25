using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace ProjectOnsMagasin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Category? category = await _categoryRepository.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: CategoryController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryRequestModel request)
        {
            Category category = new()
            {
                Name = request.Name
            };

            await _categoryRepository.Add(category);

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Category? category = await _categoryRepository.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: CategoryController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryRequestModelForUpdate request)
        {
            if (id != request.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(request);

            Category category = new Category
            {
                Id = id,
                Name = request.Name
            };

            try
            {
                await _categoryRepository.Edit(category);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: CategoryController/Delete/5
        //public ActionResult Delete(int id)
        //{return View();}

        // POST: CategoryController/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryRepository.Remove(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Search()
        {
            string? name = HttpContext.Request.Form["name"];
            var result = _categoryRepository.Search(name ?? "");
            return View("Index", result);
        }


    }
}
