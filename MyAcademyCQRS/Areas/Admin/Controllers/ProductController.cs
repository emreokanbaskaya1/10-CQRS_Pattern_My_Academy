using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAcademyCQRS.CQRSPattern.Commands.ProductCommands;
using MyAcademyCQRS.CQRSPattern.Handlers.CategoryHandlers;
using MyAcademyCQRS.CQRSPattern.Handlers.ProductHandlers;

namespace MyAcademyCQRS.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController(GetProductsQueryHandler getProductsQueryHandler,
                                   GetCategoriesQueryHandler getCategoriesQueryHandler,
                                   CreateProductCommandHandler createProductCommandHandler) : Controller
    {

        public async Task GetCategoriesAsync()
        {
            var categories = await getCategoriesQueryHandler.Handle();
            ViewBag.Categories = (from x in categories
                                  select new SelectListItem
                                  {
                                      Text= x.Name,
                                      Value = x.Id.ToString(),
                                  }).ToList();
        }


        public async Task<IActionResult> Index()
        {
            var products = await getProductsQueryHandler.Handle();
            return View(products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            await GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            await createProductCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }


    }
}
