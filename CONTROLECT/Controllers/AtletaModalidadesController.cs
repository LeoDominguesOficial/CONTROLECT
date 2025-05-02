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
    public class AtletaModalidadesController : Controller
    {
        private readonly AppDbContext _context;

        public AtletaModalidadesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AtletaModalidades
        //public async Task<IActionResult> Index()
        //{
        //    var appDbContext = _context.AtletaModalidade.Include(a => a.IdAtletaNavigation).Include(a => a.IdModalidadeNavigation);
        //    return View(await appDbContext.OrderBy(a=>a.IdModalidadeNavigation.NomeModalidade).ThenBy(a=> a.IdAtletaNavigation.NomeCompleto).ToListAsync());
        //}


        public async Task<IActionResult> Index(string sortOrder, string nomeAtleta, int modalidadeId, int idTurno, int meses)
        {
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a=>a.Ativo == true).OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto");
            ViewData["ModalidadeId"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");

            ViewData["IdTurno"] = new SelectList(_context.Turno.OrderBy(a => a.IdTurno), "IdTurno", "NomeTurno");


            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewData["FiltroAtleta"] = nomeAtleta;
            ViewData["FiltroModalidade"] = modalidadeId;


            var atletaModalidades = from s in _context.AtletaModalidade.Include(m => m.IdAtletaNavigation).Include(m => m.IdModalidadeNavigation).Include(m=>m.IdTurnoNavigation).OrderBy(s => s.IdAtletaNavigation.NomeCompleto)
                                    select s;

            if (!String.IsNullOrEmpty(nomeAtleta))
            {
                atletaModalidades = atletaModalidades.Where(s => s.IdAtletaNavigation.NomeCompleto.Contains(nomeAtleta)).OrderBy(s => s.IdAtletaNavigation.NomeCompleto);
            }

            if (modalidadeId > 0)
            {
                atletaModalidades = atletaModalidades.Where(s => s.IdModalidadeNavigation.IdModalidade == modalidadeId).OrderBy(s=>s.IdAtletaNavigation.NomeCompleto);
            }

            if (idTurno > 0)
            {
                atletaModalidades = atletaModalidades.Where(s => s.IdTurno == idTurno).OrderBy(s => s.IdAtletaNavigation.NomeCompleto);
            }


            switch (sortOrder)
            {
                case "name_desc":
                    atletaModalidades = atletaModalidades.OrderByDescending(s => s.IdAtletaNavigation.PrimeiroNome);
                    break;
                case "Date":
                    atletaModalidades = atletaModalidades.OrderBy(s => s.IdAtletaNavigation.PrimeiroNome);
                    break;
                case "date_desc":
                    atletaModalidades = atletaModalidades.OrderByDescending(s => s.IdAtletaNavigation.PrimeiroNome);
                    break;
                default:
                    atletaModalidades = atletaModalidades.OrderByDescending(s => s.IdModalidadeNavigation.NomeModalidade).ThenBy(s=>s.IdAtletaNavigation.NomeCompleto);
                    break;
            }
            return View(await atletaModalidades.AsNoTracking().ToListAsync());
        }



        // GET: AtletaModalidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atletaModalidade = await _context.AtletaModalidade
                .Include(a => a.IdAtletaNavigation)
                .Include(a => a.IdModalidadeNavigation)
                .Include(a=> a.IdTurnoNavigation)
                .FirstOrDefaultAsync(m => m.IdAtletaModalidade == id);
            if (atletaModalidade == null)
            {
                return NotFound();
            }

            return View(atletaModalidade);
        }

        // GET: AtletaModalidades/Create
        public IActionResult Create()
        {
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true).OrderBy(a=>a.NomeCompleto), "IdAtleta", "NomeCompleto");
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a=> a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdTurno"] = new SelectList(_context.Turno.OrderBy(a => a.IdTurno), "IdTurno", "NomeTurno");

            return View();
        }

        // POST: AtletaModalidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAtletaModalidade,IdAtleta,IdModalidade,Ativo,AtividadePrincipal,IdTurno")] AtletaModalidade atletaModalidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(atletaModalidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true), "IdAtleta", "NomeCompleto", atletaModalidade.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", atletaModalidade.IdModalidade);
            return View(atletaModalidade);
        }

        // GET: AtletaModalidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atletaModalidade = await _context.AtletaModalidade.FindAsync(id);
            if (atletaModalidade == null)
            {
                return NotFound();
            }
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a=>a.Ativo == true), "IdAtleta", "NomeCompleto", atletaModalidade.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", atletaModalidade.IdModalidade);
            ViewData["IdTurno"] = new SelectList(_context.Turno.OrderBy(a => a.IdTurno), "IdTurno", "NomeTurno");

            return View(atletaModalidade);
        }

        // POST: AtletaModalidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAtletaModalidade,IdAtleta,IdModalidade,Ativo,AtividadePrincipal,IdTurno")] AtletaModalidade atletaModalidade)
        {
            if (id != atletaModalidade.IdAtletaModalidade)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atletaModalidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtletaModalidadeExists(atletaModalidade.IdAtletaModalidade))
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
            ViewData["IdAtleta"] = new SelectList(_context.Atleta, "IdAtleta", "NomeCompleto", atletaModalidade.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade, "IdModalidade", "NomeModalidade", atletaModalidade.IdModalidade);
            return View(atletaModalidade);
        }

        // GET: AtletaModalidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atletaModalidade = await _context.AtletaModalidade
                .Include(a => a.IdAtletaNavigation)
                .Include(a => a.IdModalidadeNavigation)
                .FirstOrDefaultAsync(m => m.IdAtletaModalidade == id);
            if (atletaModalidade == null)
            {
                return NotFound();
            }

            return View(atletaModalidade);
        }

        // POST: AtletaModalidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atletaModalidade = await _context.AtletaModalidade.FindAsync(id);
            _context.AtletaModalidade.Remove(atletaModalidade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtletaModalidadeExists(int id)
        {
            return _context.AtletaModalidade.Any(e => e.IdAtletaModalidade == id);
        }
    }
}
