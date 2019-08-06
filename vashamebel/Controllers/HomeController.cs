using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vashamebel.Models;
using vashamebel.Models.Admin;

namespace vashamebel.Controllers
{
    public class HomeController : Controller
    {

        
        public IDataRepository repository;

        public HomeController(IDataRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(string name = null, decimal? price = null)
        {
            var products = repository.GetFilteredProducts(name, price);
            ViewBag.name = name;
            ViewBag.price = price;
            return View(products);
        }


        //modal
        public ActionResult Details(long id)
        {
            ViewBag.CreateMode = false;
            return View("Details", repository.GetProduct(id));
        }




        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
