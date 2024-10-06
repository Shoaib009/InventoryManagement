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
        public IActionResult User()
        {
            //var UserData = 
            return View();
        }

        public IActionResult Registration()
        {
            //var UserData = 
            return View();
        }

        [HttpPost]

        public JsonResult UserCreation(UserCreation Obj)
        {
            if (Obj == null || string.IsNullOrEmpty(Obj.Email) || string.IsNullOrEmpty(Obj.Password))
            {
                return new JsonResult(new { success = false, message = "PLease enter email and password." });
            }
            var newUser = _databaseContext.Users.FirstOrDefault(x => x.Email.ToLower().Equals(Obj.Email.ToLower()));
            var newUserPassword = _databaseContext.Users.FirstOrDefault(x => x.Password.ToLower().Equals(Obj.Password.ToLower()));

            if (newUser != null && newUserPassword != null)
            {
                return new JsonResult(new { success = true, message = "User login successful." });
            }
            else
            {
                return new JsonResult(new { success = false, message = "User login Failed." });
            }
        }

        [HttpPost]
        public JsonResult CreateUser(CreateUserRequest Obj)
        {
            if (Obj == null || string.IsNullOrEmpty(Obj.FirstName) || string.IsNullOrEmpty(Obj.LastName) || string.IsNullOrEmpty(Obj.Email) || string.IsNullOrEmpty(Obj.ConfirmEmail) ||
                string.IsNullOrEmpty(Obj.PhoneNumber) || string.IsNullOrEmpty(Obj.Password) || string.IsNullOrEmpty(Obj.ConfirmPassword))
            {
                return new JsonResult(new { success = false, message = "Please enter the first name, last name, email, confirm email, phone number , password and confirm password." });
                //return RedirectToAction("Registration", "Home");
            }
            if (Obj.Email != Obj.ConfirmEmail)
            {
                return new JsonResult(new { success = false, message = "email and confirm email are not equal." });
            }
            if (Obj.Password != Obj.ConfirmPassword)
            {
                return new JsonResult(new { success = false, message = "password and confirm password are not equal." });
            }
            var newUser = _databaseContext.Users.FirstOrDefault(x => x.Email.ToLower().Equals(Obj.Email.ToLower()));
            if (newUser != null)
            {
                return new JsonResult(new { success = false, message = "this email already exist. please type new eamil." });
            }
            var Phone = Convert.ToInt64(Obj.PhoneNumber);
            var addUser = new User
            {
                FirstName = Obj.FirstName,
                LastName = Obj.LastName,
                Email = Obj.Email,
                ConfirmEmail = Obj.ConfirmEmail,
                PhoneNumber = Phone,
                Password = Obj.Password,
                ConfirmPassword = Obj.ConfirmPassword
            };
            _databaseContext.Users.Add(addUser);
            _databaseContext.SaveChanges();
            return new JsonResult(new { success = true, message = "User added successfully. Congrats!" });
        }

        [HttpPost]
        public JsonResult CreateCategory([FromBody] CreateTaskRequest Req)
        {
            if (Req == null || string.IsNullOrWhiteSpace(Req.Name))
            {
                return new JsonResult(new { success = false, message = "please enter category name." });
            }
            var existingCategory = _databaseContext.Categories.FirstOrDefault(x => x.Name.ToLower().Equals(Req.Name.ToLower()));
            if (existingCategory != null && existingCategory.IsDeleted == false)
            {
                return new JsonResult(new { success = false, message = "This category name already exists. Please enter a new category name." });
            }
            else if (existingCategory != null && existingCategory.IsDeleted == true)
            {
                existingCategory.IsDeleted = false;
                _databaseContext.Categories.Update(existingCategory);
                _databaseContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Category added successfully." });

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

            var existingProduct = _databaseContext.Products.FirstOrDefault(x => x.Name.ToLower().Equals(requestProduct.Name.ToLower()));
            if (existingProduct != null && existingProduct.IsDeleted == false)
            {
                return new JsonResult(new { success = false, message = "This Product name already exists. Please enter a new product name." });
            }
            else if (existingProduct != null && existingProduct.IsDeleted == true)
            {
                existingProduct.IsDeleted = false;
                _databaseContext.Products.Update(existingProduct);
                _databaseContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Product added successfully. Congrats!." });

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
