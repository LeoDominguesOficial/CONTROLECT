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
    public class ExameFaixasController : Controller
    {
        private readonly AppDbContext _context;

        public ExameFaixasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ExameFaixas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExameFaixa.OrderBy(a=>a.NomeExameFaixa).ToListAsync());
        }

        // GET: ExameFaixas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ExameFaixa = await _context.ExameFaixa
                .FirstOrDefaultAsync(m => m.IdExameFaixa == id);
            if (ExameFaixa == null)
            {
                return NotFound();
            }

            return View(ExameFaixa);
        }

        // GET: ExameFaixas/Create
        public IActionResult Create()
        {
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");

            return View();
        }

        // POST: ExameFaixas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExameFaixa,IdModalidade,NomeExameFaixa,DataInicial,DataFinal,Ativo")] ExameFaixa ExameFaixa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ExameFaixa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ExameFaixa);
        }

        // GET: ExameFaixas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ExameFaixa = await _context.ExameFaixa.FindAsync(id);
            if (ExameFaixa == null)
            {
                return NotFound();
            }

            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", ExameFaixa.IdModalidade);

            return View(ExameFaixa);
        }

        // POST: ExameFaixas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExameFaixa,IdModalidade,NomeExameFaixa,DataInicial,DataFinal,Ativo")] ExameFaixa ExameFaixa)
        {
            if (id != ExameFaixa.IdExameFaixa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ExameFaixa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExameFaixaExists(ExameFaixa.IdExameFaixa))
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
            return View(ExameFaixa);
        }

        // GET: ExameFaixas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ExameFaixa = await _context.ExameFaixa
                .FirstOrDefaultAsync(m => m.IdExameFaixa == id);
            if (ExameFaixa == null)
            {
                return NotFound();
            }

            return View(ExameFaixa);
        }

        // POST: ExameFaixas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ExameFaixa = await _context.ExameFaixa.FindAsync(id);
            _context.ExameFaixa.Remove(ExameFaixa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExameFaixaExists(int id)
        {
            return _context.ExameFaixa.Any(e => e.IdExameFaixa == id);
        }
    }
}
