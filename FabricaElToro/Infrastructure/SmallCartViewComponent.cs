using FabricaElToro.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabricaElToro.Infrastructure
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel smallCartVW;

            if(cart == null || cart.Count == 0)
            {
                smallCartVW = null;
            }
            else
            {
                smallCartVW = new SmallCartViewModel
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmout = cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(smallCartVW);      
        }
    }
}
