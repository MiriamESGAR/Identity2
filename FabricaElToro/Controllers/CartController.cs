﻿using FabricaElToro.Infrastructure;
using FabricaElToro.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabricaElToro.Controllers
{
    public class CartController : Controller
    {
        private readonly FabricaElToroContext context;
        public CartController(FabricaElToroContext context)
        {
            this.context = context;
        }
        //GET / cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            
            return View(cartVM);
        }
        //GET / cart/agregar/5
        public async Task< IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if(cartItem == null)
            {
                cart.Add(new CartItem (product));
            }
            else
            {
                cartItem.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            return RedirectToAction("Index");
        }

        //GET / cart/decrease/5
        public async Task<IActionResult> Decrease(int id)
        {
            
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }
           

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }


            return RedirectToAction("Index");
        }
        //GET / cart/remove/5
        public async Task<IActionResult> Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            
             cart.RemoveAll(x => x.ProductId == id);
            


            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }


            return RedirectToAction("Index");
        }

        //GET / cart/clear
        public async Task<IActionResult> Clear()
        {       
                HttpContext.Session.Remove("Cart");
                 return RedirectToAction("Index");
        }

    }
}

