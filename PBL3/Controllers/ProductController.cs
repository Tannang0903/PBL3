﻿using Models.BLL;
using Models.DAL;
using Models.DTO;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int id)
        {
            Product product = new ProductBO().find(id);
            if(product == null)
            {
                return RedirectToAction("Index", new
                {
                    controller = "Home"
                });
            }
            else
            {
                ViewBag.Comments = new CommentBO().getCommentsOfProduct(id);
                ViewBag.Rates = new
                {
                     rating = (ViewBag.Comments as List<Comment>).GroupBy(i => i.Rate).Select(x => new
                     {
                         rate = x.Key,
                         quantity = x.Count()
                     }).ToList(),
                };
                //dynamic o = new
                //{
                //    rating = (ViewBag.Comments as List<Comment>).GroupBy(i => i.Rate).Select(x => new
                //    {
                //        rate = x.Key,
                //        quantity = x.Count()
                //    }),
                //};
                return View(product);
                

            }
        }
        public ActionResult Search(string keyword = "", string categoryID = "All", string price = "All", int page = 1)
        {
            ProductBO productDAO = new ProductBO();
            int totalPage = 0;
            List<Product> products = productDAO.getPage(page, 20, keyword, categoryID, price, out totalPage);
            ViewBag.categories = new CategoryBO().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&CategoryID={categoryID}&Price={price}"
            };
            return View(products);
        }
    }
}