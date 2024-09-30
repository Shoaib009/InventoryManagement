using InventoryManagement.Models;
using InventoryManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _databaseContext;

        public HomeController(ILogger<HomeController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Product()
        {
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Products = _databaseContext.Products.Where(x => x.IsDeleted == false).ToList();
            productViewModel.Category = _databaseContext.Categories.Where(x => x.IsDeleted == false).ToList();
            return View(productViewModel);
        }
        public IActionResult Category()
        {
            var Inventory = _databaseContext.Categories.Where(x => x.IsDeleted == false).ToList();
            return View(Inventory);
        }
        [HttpPost]
        public JsonResult CreateCategory([FromBody] CreateTaskRequest Req)
        {
            if (Req == null || string.IsNullOrEmpty(Req.Name))
            {
                return new JsonResult(new { success = false, message = "please enter category name." });
            }
            else
            {
                var AddCategory = new Category { Name = Req.Name };
                _databaseContext.Categories.Add(AddCategory);
                _databaseContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Category added successfully." });
            }
        }

        [HttpPost]
        public JsonResult DeleteCategory([FromBody] DeleteTaskRequest Req)
        {
            if (Req == null || Req.id == 0)
            {
                return new JsonResult(new { success = false, message = "Category not found" });
            }
            else
            {
                var DeleteCategory = _databaseContext.Categories.Where(x => x.Id == Req.id).FirstOrDefault();
                if (DeleteCategory == null)
                {
                    return new JsonResult(new { success = false, message = "Category not found." });
                }
                else
                {
                    DeleteCategory.IsDeleted = true;
                    _databaseContext.Categories.Update(DeleteCategory);
                    _databaseContext.SaveChanges();
                    return new JsonResult(new { success = true, message = "Category deleted successfully." });
                }
            }
        }

        [HttpPost]
        public JsonResult UpdateCategory([FromBody] UpdateTaskRequest Req)
        {
            if (Req == null || Req.id == 0 || string.IsNullOrEmpty(Req.Name))
            {
                return new JsonResult(new { success = false, message = "please enter category name" });
            }
            else
            {
                var UpdateCategory = _databaseContext.Categories.Where(x => x.Id == Req.id).FirstOrDefault();
                if (UpdateCategory == null)
                {
                    return new JsonResult(new { success = false, message = "please enter category name" });
                }
                else
                {
                    UpdateCategory.Name = Req.Name;
                    _databaseContext.Categories.Update(UpdateCategory);
                    _databaseContext.SaveChanges();
                    return new JsonResult(new { success = true, message = "Category updated successfully." });
                }
            }
        }
        [HttpPost]
        public JsonResult CreateProduct([FromBody] CreateTaskRequestProduct requestProduct)
        {
            if (requestProduct == null || string.IsNullOrEmpty(requestProduct.Name) || requestProduct.Price == 0 || requestProduct.Quantity == 0 || requestProduct.CategoryId == 0)
            {
                return new JsonResult(new { success = false, message = "Please enter the procuct name, price and quantity." });
            }
            else
            {
                var AddProduct = new Product { Name = requestProduct.Name, Price = requestProduct.Price, Quantity = requestProduct.Quantity, CategoryId = requestProduct.CategoryId };
                _databaseContext.Products.Add(AddProduct);
                _databaseContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Product added successfully. Congrats!" });
            }

        }
        [HttpPost]
        public JsonResult DeleteProduct([FromBody] DeleteTaskRequestProduct Req)
        {
            if (Req == null || Req.id == 0)
            {
                return new JsonResult(new { success = false, message = "Product not found" });
            }
            else
            {
                var DeleteProduct = _databaseContext.Products.Where(x => x.Id == Req.id).FirstOrDefault();
                if (DeleteCategory == null)
                {
                    return new JsonResult(new { success = false, message = "Product not found." });
                }
                else
                {
                    DeleteProduct.IsDeleted = true;
                    _databaseContext.Products.Update(DeleteProduct);
                    _databaseContext.SaveChanges();
                    return new JsonResult(new { success = true, message = "Product deleted successfully." });
                }
            }
        }
        public JsonResult UpdateProduct([FromBody] UpdateTaskRequestProduct Req)
        {
            if (Req == null || string.IsNullOrEmpty(Req.Name) ||
                Req.Price == 0 || Req.Quantity == 0 || Req.CategoryId == 0)
            {
                return new JsonResult(new { success = false, message = "Please enter the procuct name, price, quantity and category id." });
            }
            else
            {
                var UpdateProduct = _databaseContext.Products.Where(x => x.Id == Req.Id).FirstOrDefault();
                if (UpdateProduct == null)
                {
                    return new JsonResult(new { success = false, message = "Please enter the procuct name, price, quantity and category id." });
                }
                else
                {
                    UpdateProduct.Name = Req.Name;
                    UpdateProduct.Price = Req.Price;
                    UpdateProduct.Quantity = Req.Quantity;
                    UpdateProduct.CategoryId = Req.CategoryId;
                    _databaseContext.Products.Update(UpdateProduct);
                    _databaseContext.SaveChanges();
                    return new JsonResult(new { success = true, message = "Product updated successfully. Congrats!" });
                }

            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
