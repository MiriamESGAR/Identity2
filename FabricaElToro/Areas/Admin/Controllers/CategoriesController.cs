﻿using FabricaElToro.Infrastructure;
using FabricaElToro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabricaElToro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly FabricaElToroContext context;
        public CategoriesController(FabricaElToroContext context)
        {
            this.context = context;
        }
        //GET /admin/categories
        public async Task <IActionResult> Index()
        {
            return View( await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }
        //GET /admin/categories/create
        public IActionResult Create() => View();

        //POST /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "El categoria ya existe.");
                    return View(category);
                }

                context.Add(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "La categoria ha sido agregada!";

                return RedirectToAction("Index");
            }

            return View(category);
        }
        //GET /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //POST /admin/categorias/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");


                var slug = await context.Categories.Where(x => x.Id !=id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Esta categoria ya existe.");
                    return View(category);
                }

                context.Update(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "La categoria ha sido editada!";

                return RedirectToAction("Edit", new { id });
            }

            return View(category);
        }

        //GET /admin/category/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "La categoria no existe!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "La categoria ha sido eliminada!";
            }
            return RedirectToAction("Index");
        }

        //POST /admin/pages/categories
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var CategoryId in id)
            {
                Category category = await context.Categories.FindAsync(CategoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();

        }
    }

}
