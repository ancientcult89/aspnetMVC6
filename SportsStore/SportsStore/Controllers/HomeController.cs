﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public int pageSize = 3;

        public HomeController(IStoreRepository repository)
        { 
            _repository = repository;
        }
        public ViewResult Index(string? category, int productPage = 1) => View(new ProductListViewModel
        {
            Products = _repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize),
            PageInfo = new PageInfo {
                CurrentPage = productPage,
                ItemsPerPage = pageSize,
                TotalItems = category == null 
                    ? _repository.Products.Count() 
                    : _repository.Products.Where(p => p.Category == category).Count()
            },
            CurrentCategory = category
        });
    }
}
