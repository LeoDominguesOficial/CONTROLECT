using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONTROLECT.Models;
using PagedList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CONTROLECT.Controllers
{
    public class AtletasController : Controller
    {
        private readonly AppDbContext _context;

        public AtletasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Atletas
        public IActionResult Index(string sortOrder, string nomeAtleta, string apelido, bool situacaoFiltro, int idModalidade, int? pagina)
        {
            int paginaTamanho = 20;
            int paginaNumero = (pagina ?? 1);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            //ViewData["IdModalidade"] = new SelectList(_context.Modalidade.OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");

            ViewData["FiltroAtleta"] = nomeAtleta;
            ViewData["FiltroApelido"] = apelido;
            ViewData["SituacaoFiltro"] = situacaoFiltro;

            if (TempData["PrimeiroAcesso"] is true)
            {
                ViewData["SituacaoFiltro"] = true;
                TempData["PrimeiroAcesso"] = false;
                situacaoFiltro = true;
            }
            else

            {
                //TempData["PrimeiroAcesso"] = true;
                //ViewData["SituacaoFiltro"] = true;
                TempData["PrimeiroAcesso"] = false;

            }

            var atletas = from s in _context.Atleta
                               select s;

            if (!String.IsNullOrEmpty(nomeAtleta))
            {
                atletas = atletas.Where(s => s.NomeCompleto.ToUpper().Contains(nomeAtleta.ToUpper()));
            }

            if (!String.IsNullOrEmpty(apelido))
            {
                atletas = atletas.Where(s => s.Apelido.ToUpper().Contains(apelido.ToUpper()));
            }

            if (situacaoFiltro)
            {
                atletas = atletas.Where(s => s.Ativo.Equals(situacaoFiltro));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    atletas = atletas.OrderByDescending(s => s.NomeCompleto);
                    break;
                case "Date":
                    atletas = atletas.OrderBy(s => s.NomeCompleto);
                    break;
                case "date_desc":
                    atletas = atletas.OrderByDescending(s => s.NomeCompleto);
                    break;
                default:
                    atletas = atletas.OrderBy(s => s.NomeCompleto);
                    break;
            }

            var skip = (paginaNumero - 1) * paginaTamanho;
            atletas.Skip(skip).Take(paginaTamanho);

            return View(atletas.ToPagedList(paginaNumero, paginaTamanho));
            //return View(await atletas.AsNoTracking().ToListAsync());
        }


        // GET: Atletas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");

            var atleta = await _context.Atleta
                .FirstOrDefaultAsync(m => m.IdAtleta == id);
            if (atleta == null)
            {
                return NotFound();
            }

            return View(atleta);
        }

        // GET: Atletas/Create
        public IActionResult Create()
        {
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            return View();
        }

        // POST: Atletas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAtleta,PrimeiroNome,UltimoNome,NomeCompleto,NomeResponsavel,Identidade,Cpf,Telefone,Ddd,Celular,Email,Endereco,Bairro,Numero,Cep,IdModalidade,Apelido,DataCadastro,Ativo")] Atleta atleta)
        {
           if (ModelState.IsValid)
            {
                atleta.DataCadastro = DateTime.Now;
                _context.Add(atleta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(atleta);
        }

        // GET: Atletas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");

            var atleta = await _context.Atleta.FindAsync(id);
            if (atleta == null)
            {
                return NotFound();
            }
            return View(atleta);
        }

        // POST: Atletas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAtleta,PrimeiroNome,UltimoNome,NomeCompleto,NomeResponsavel,Identidade,Cpf,Telefone,Ddd,Celular,Email,Endereco,Bairro,Numero,Cep,Apelido,DataCadastro,Ativo")] Atleta atleta)
        {
            if (id != atleta.IdAtleta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atleta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtletaExists(atleta.IdAtleta))
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
            return View(atleta);
        }

        // GET: Atletas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atleta = await _context.Atleta
                .FirstOrDefaultAsync(m => m.IdAtleta == id);
            if (atleta == null)
            {
                return NotFound();
            }

            return View(atleta);
        }

        // POST: Atletas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atleta = await _context.Atleta.FindAsync(id);
            _context.Atleta.Remove(atleta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtletaExists(int id)
        {
            return _context.Atleta.Any(e => e.IdAtleta == id);
        }
    }
}
