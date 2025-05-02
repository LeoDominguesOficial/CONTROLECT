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
    public class ProfessorModalidadesController : Controller
    {
        private readonly AppDbContext _context;

        public ProfessorModalidadesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProfessorModalidades
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ProfessorModalidade.Include(p => p.IdModalidadeNavigation).Include(p => p.IdProfessorNavigation);
            return View(await appDbContext.OrderBy(a=>a.IdModalidadeNavigation.NomeModalidade).ToListAsync());
        }

        // GET: ProfessorModalidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professorModalidade = await _context.ProfessorModalidade
                .Include(p => p.IdModalidadeNavigation)
                .Include(p => p.IdProfessorNavigation)
                .FirstOrDefaultAsync(m => m.IdProfessorModalidade == id);
            if (professorModalidade == null)
            {
                return NotFound();
            }

            return View(professorModalidade);
        }

        // GET: ProfessorModalidades/Create
        public IActionResult Create()
        {
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a=>a.Ativo == true).OrderBy(a=>a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdProfessor"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a=>a.NomeProfessor), "IdProfessor", "NomeProfessor");
            return View();
        }

        // POST: ProfessorModalidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProfessorModalidade,IdProfessor,IdModalidade,Ativo,DataOperacao")] ProfessorModalidade professorModalidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professorModalidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade, "IdModalidade", "NomeModalidade", professorModalidade.IdModalidade);
            ViewData["IdProfessor"] = new SelectList(_context.Professor, "IdProfessor", "NomeProfessor", professorModalidade.IdProfessor);
            return View(professorModalidade);
        }

        // GET: ProfessorModalidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professorModalidade = await _context.ProfessorModalidade.FindAsync(id);
            if (professorModalidade == null)
            {
                return NotFound();
            }
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade, "IdModalidade", "NomeModalidade", professorModalidade.IdModalidade);
            ViewData["IdProfessor"] = new SelectList(_context.Professor, "IdProfessor", "NomeProfessor", professorModalidade.IdProfessor);
            return View(professorModalidade);
        }

        // POST: ProfessorModalidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProfessorModalidade,IdProfessor,IdModalidade,Ativo,DataOperacao")] ProfessorModalidade professorModalidade)
        {
            if (id != professorModalidade.IdProfessorModalidade)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professorModalidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorModalidadeExists(professorModalidade.IdProfessorModalidade))
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
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade, "IdModalidade", "NomeModalidade", professorModalidade.IdModalidade);
            ViewData["IdProfessor"] = new SelectList(_context.Professor, "IdProfessor", "NomeProfessor", professorModalidade.IdProfessor);
            return View(professorModalidade);
        }

        // GET: ProfessorModalidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professorModalidade = await _context.ProfessorModalidade
                .Include(p => p.IdModalidadeNavigation)
                .Include(p => p.IdProfessorNavigation)
                .FirstOrDefaultAsync(m => m.IdProfessorModalidade == id);
            if (professorModalidade == null)
            {
                return NotFound();
            }

            return View(professorModalidade);
        }

        // POST: ProfessorModalidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var professorModalidade = await _context.ProfessorModalidade.FindAsync(id);
            _context.ProfessorModalidade.Remove(professorModalidade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorModalidadeExists(int id)
        {
            return _context.ProfessorModalidade.Any(e => e.IdProfessorModalidade == id);
        }
    }
}
