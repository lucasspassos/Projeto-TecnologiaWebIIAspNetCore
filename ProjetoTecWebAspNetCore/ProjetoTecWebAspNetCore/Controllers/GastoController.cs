﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoTecWebAspNetCore.Models;

namespace ProjetoTecWebAspNetCore.Controllers
{
    public class GastoController : Controller
    {
        private readonly AppContextModel _context;

        public GastoController(AppContextModel context)
        {
            _context = context;
        }

        // GET: Gasto
        public async Task<IActionResult> Index()
        {
            var appContextModel = _context.Gastos.Include(g => g.Conta);
            return View(await appContextModel.ToListAsync());
        }

        // GET: Gasto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gastosModel = await _context.Gastos
                .Include(g => g.Conta)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gastosModel == null)
            {
                return NotFound();
            }

            return View(gastosModel);
        }

        // GET: Gasto/Create
        public IActionResult Create()
        {
            ViewData["ContaID"] = new SelectList(_context.Contas, "ID", "ID");
            return View();
        }

        // POST: Gasto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ContaID,Custo,Categoria")] GastosModel gastosModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gastosModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaID"] = new SelectList(_context.Contas, "ID", "ID", gastosModel.ContaID);
            return View(gastosModel);
        }

        // GET: Gasto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gastosModel = await _context.Gastos.FindAsync(id);
            if (gastosModel == null)
            {
                return NotFound();
            }
            ViewData["ContaID"] = new SelectList(_context.Contas, "ID", "ID", gastosModel.ContaID);
            return View(gastosModel);
        }

        // POST: Gasto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ContaID,Custo,Categoria")] GastosModel gastosModel)
        {
            if (id != gastosModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gastosModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastosModelExists(gastosModel.ID))
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
            ViewData["ContaID"] = new SelectList(_context.Contas, "ID", "ID", gastosModel.ContaID);
            return View(gastosModel);
        }

        // GET: Gasto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gastosModel = await _context.Gastos
                .Include(g => g.Conta)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gastosModel == null)
            {
                return NotFound();
            }

            return View(gastosModel);
        }

        // POST: Gasto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gastosModel = await _context.Gastos.FindAsync(id);
            _context.Gastos.Remove(gastosModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastosModelExists(int id)
        {
            return _context.Gastos.Any(e => e.ID == id);
        }
    }
}
