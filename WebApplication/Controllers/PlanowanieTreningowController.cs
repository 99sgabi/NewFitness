﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class PlanowanieTreningowController : Controller
    {
        private readonly MyContext _context;

        public PlanowanieTreningowController(MyContext context)
        {
            _context = context;
        }

        // GET: PlanowanieTreningow
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.Identity.GetUserId());
            var myContext = _context.planowaneTreningi.Include(p => p.trening).Where(x => x.id_uzytkownika == userId );
            return View(await myContext.ToListAsync());
        }

        // GET: PlanowanieTreningow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.Identity.GetUserId());
            var planowanieTreningow = await _context.planowaneTreningi
                .Include(p => p.trening)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planowanieTreningow == null)
            {
                return NotFound();
            }

            if (planowanieTreningow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            return View(planowanieTreningow);
        }

        
        // GET: PlanowanieTreningow/Create
        public IActionResult Create(string id_string)
        {
            ViewData["id_treningu"] = new SelectList(_context.treningi, "id_treningu", "nazwa");
            if (!String.IsNullOrEmpty(id_string))
            {
                int id = int.Parse(id_string);
                PlanowanieTreningow training = new PlanowanieTreningow();
                training.id_treningu = id;
                training.data = DateTime.Now;
                //training.trening = _context.treningi.Where(x => x.id_treningu == id).FirstOrDefault();
                return View(training);
            }

            return View();
        }

        // POST: PlanowanieTreningow/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_uzytkownika,id_treningu,data")] PlanowanieTreningow planowanieTreningow)
        {
            if (ModelState.IsValid)
            {
                planowanieTreningow.id_uzytkownika = int.Parse(User.Identity.GetUserId());
                _context.Add(planowanieTreningow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_treningu"] = new SelectList(_context.treningi, "id_treningu", "nazwa", planowanieTreningow.id_treningu);
            return View(planowanieTreningow);
        }

        /*/ GET: PlanowanieTreningow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planowanieTreningow = await _context.planowaneTreningi.FindAsync(id);
            if (planowanieTreningow == null)
            {
                return NotFound();
            }
            ViewData["id_treningu"] = new SelectList(_context.treningi, "id_treningu", "nazwa", planowanieTreningow.id_treningu);
            ViewData["id_uzytkownika"] = new SelectList(_context.uzytkownicy, "Id", "Id", planowanieTreningow.id_uzytkownika);
            return View(planowanieTreningow);
        }

        // POST: PlanowanieTreningow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_uzytkownika,id_treningu,data")] PlanowanieTreningow planowanieTreningow)
        {
            if (id != planowanieTreningow.id_treningu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planowanieTreningow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanowanieTreningowExists(planowanieTreningow.id_treningu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_treningu"] = new SelectList(_context.treningi, "id_treningu", "nazwa", planowanieTreningow.id_treningu);
            ViewData["id_uzytkownika"] = new SelectList(_context.uzytkownicy, "Id", "Id", planowanieTreningow.id_uzytkownika);
            return View(planowanieTreningow);
        }*/

        // GET: PlanowanieTreningow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planowanieTreningow = await _context.planowaneTreningi
                .Include(p => p.trening)
                .Include(p => p.uzytkownik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (planowanieTreningow == null)
            {
                return NotFound();
            }

            if (planowanieTreningow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            return View(planowanieTreningow);
        }

        // POST: PlanowanieTreningow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planowanieTreningow = await _context.planowaneTreningi.FindAsync(id);

            if (planowanieTreningow.id_uzytkownika != int.Parse(User.Identity.GetUserId()))
                return RedirectToAction("Index");

            _context.planowaneTreningi.Remove(planowanieTreningow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanowanieTreningowExists(int id)
        {
            return _context.planowaneTreningi.Any(e => e.id_treningu == id);
        }
    }
}
