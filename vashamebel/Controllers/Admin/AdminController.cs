using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using vashamebel.Models.Admin;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using vashamebel.Models;
using vashamebel.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace vashamebel.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationContext context;
        public IDataRepository repository;
        private IHostingEnvironment _appEnvironment;

        public AdminController( IDataRepository repo, IHostingEnvironment appEnvironment, ApplicationContext ctx)
        {
            context = ctx;
            repository = repo;
            _appEnvironment = appEnvironment;
        }
       
        public IActionResult Index(string name = null, decimal? price = null)
        {
            var products = repository.GetFilteredProducts(name, price);
            ViewBag.category = name;
            ViewBag.price = price;
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.CreateMode = true;
            return View("Editor", new Product());
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                product.NameImage = uploadedFile.FileName;
                product.Path = path;
            }
            repository.CreateProduct(product);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(long id)
        {
            ViewBag.CreateMode = false;
            return View("Editor", repository.GetProduct(id));
        }

        //Загрузка файлов на сервер
        [HttpPost]
        public async Task <IActionResult> Edit(Product product, Product original, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                product.NameImage = uploadedFile.FileName;
                product.Path = path;
            }
            repository.UpdateProduct(product, original);
            return RedirectToAction(nameof(Index));
        }

       

        [HttpPost]
        public IActionResult Delete(long id)
        {
            repository.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

