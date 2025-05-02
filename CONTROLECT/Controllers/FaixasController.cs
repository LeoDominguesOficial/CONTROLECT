using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONTROLECT.Models;

namespace CONTROLECT.Controllers
{
    public class FaixasController : Controller
    {
        private readonly AppDbContext _context;

        public FaixasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Faixas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faixa.OrderBy(a=>a.IdFaixa).ToListAsync());
        }

        // GET: Faixas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Faixa = await _context.Faixa
                .FirstOrDefaultAsync(m => m.IdFaixa == id);
            if (Faixa == null)
            {
                return NotFound();
            }

            return View(Faixa);
        }

        // GET: Faixas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faixas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFaixa,NomeFaixa")] Faixa Faixa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Faixa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Faixa);
        }

        // GET: Faixas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Faixa = await _context.Faixa.FindAsync(id);
            if (Faixa == null)
            {
                return NotFound();
            }
            return View(Faixa);
        }

        // POST: Faixas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFaixa,NomeFaixa")] Faixa Faixa)
        {
            if (id != Faixa.IdFaixa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Faixa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaixaExists(Faixa.IdFaixa))
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
            return View(Faixa);
        }

        // GET: Faixas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Faixa = await _context.Faixa
                .FirstOrDefaultAsync(m => m.IdFaixa == id);
            if (Faixa == null)
            {
                return NotFound();
            }

            return View(Faixa);
        }

        // POST: Faixas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Faixa = await _context.Faixa.FindAsync(id);
            _context.Faixa.Remove(Faixa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaixaExists(int id)
        {
            return _context.Faixa.Any(e => e.IdFaixa == id);
        }
    }
}
