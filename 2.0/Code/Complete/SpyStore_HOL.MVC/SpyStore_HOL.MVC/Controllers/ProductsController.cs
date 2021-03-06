﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyStore_HOL.DAL.Repos.Interfaces;
using SpyStore_HOL.MVC.Controllers.Base;
using SpyStore_HOL.MVC.Support;

namespace SpyStore_HOL.MVC.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryRepo _categoryRepo;

        private readonly CustomSettings _settings;

        public ProductsController(IProductRepo productRepo, ICategoryRepo categoryRepo, 
            IOptionsSnapshot<CustomSettings> settings,ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            Logger = logger;
        }
        public ILogger Logger { get; }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            Logger.LogInformation(1, "Enter About");
            return RedirectToAction(nameof(Featured));
        }

        public ActionResult Details(int id)
        {
            return RedirectToAction(nameof(CartController.AddToCart), nameof(CartController).Replace("Controller", ""), new { customerId = ViewBag.CustomerId, productId = id, cameFromProducts = true });
        }


        [HttpGet]
        public IActionResult Featured()
        {
            ViewBag.Title = "Featured Products";
            ViewBag.Header = "Featured Products";
            ViewBag.ShowCategory = true;
            ViewBag.Featured = true;
            return View("ProductList", _productRepo.GetFeaturedWithCategoryName());
        }

        [HttpGet]
        public IActionResult ProductList(int id)
        {
            var cat = _categoryRepo.Find(id);
            ViewBag.Title = cat?.CategoryName;
            ViewBag.Header = cat?.CategoryName;
            ViewBag.ShowCategory = false;
            ViewBag.Featured = false;
            return View(_productRepo.GetProductsForCategory(id));
        }


        [Route("[controller]/[action]")]
        [HttpPost("{searchString}")]
        public IActionResult Search(string searchString)
        {
            ViewBag.Title = "Search Results";
            ViewBag.Header = "Search Results";
            ViewBag.ShowCategory = true;
            ViewBag.Featured = false;
            return View("ProductList", _productRepo.Search(searchString));
        }
    }
}
