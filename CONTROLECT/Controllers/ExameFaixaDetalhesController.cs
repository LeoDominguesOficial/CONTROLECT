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
    public class ExameFaixaDetalhesController : Controller
    {
        private readonly AppDbContext _context;

        public ExameFaixaDetalhesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(int IdExameFaixa)
        {
            //int mesAtual = DateTime.Now.Month;
            //ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);

            ViewData["IdExameFaixa"] = new SelectList(_context.ExameFaixa.Where(a => a.Ativo == true).OrderByDescending(a => a.IdExameFaixa), "IdExameFaixa", "NomeExameFaixa");


            //ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true).OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto");
            //ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            //ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.NomeFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");

            var exameFaixaDetalhes = from exame in _context.ExameFaixaDetalhe.Include(a => a.IdExameNavigation)
                                     .Include(a=>a.IdAtletaNavigation).Include(a=>a.IdFormaPagamentoNavigation)
                                     .Where(a=>a.IdExameFaixa == IdExameFaixa)
                                     .OrderByDescending(a => a.DataPagamento) select exame;

            return View(await exameFaixaDetalhes.OrderByDescending(a=>a.IdExameFaixaDetalhe).AsNoTracking().ToListAsync());

            //return View(await _context.ExameFaixaDetalhe.Include(a=>a.IdDespesaNavigation).OrderByDescending(a=>a.DataPagamento).ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ExameFaixaDetalhe = await _context.ExameFaixaDetalhe.Include(a=>a.IdExameNavigation)
                .FirstOrDefaultAsync(m => m.IdExameFaixaDetalhe == id);
            if (ExameFaixaDetalhe == null)
            {
                return NotFound();
            }

            return View(ExameFaixaDetalhe);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["IdExameFaixa"] = new SelectList(_context.ExameFaixa.Where(a => a.Ativo == true).OrderBy(a => a.NomeExameFaixa), "IdExameFaixa", "NomeExameFaixa");
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true).OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto");
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.NomeFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExameFaixaDetalhe,IdExameFaixa,IdAtleta,DataPagamento,Valor, Observacao, IdFormaPagamento")] ExameFaixaDetalhe ExameFaixaDetalhe)
        {
            if (ModelState.IsValid)
            {

                _context.Add(ExameFaixaDetalhe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ExameFaixaDetalhe);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["IdExameFaixa"] = new SelectList(_context.ExameFaixa.Where(a => a.Ativo == true).OrderBy(a => a.NomeExameFaixa), "IdExameFaixa", "NomeExameFaixa");
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true).OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto");
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.NomeFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");

            var ExameFaixaDetalhe = await _context.ExameFaixaDetalhe.FindAsync(id);


            if (ExameFaixaDetalhe == null)
            {
                return NotFound();
            }

            

            return View(ExameFaixaDetalhe);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExameFaixaDetalhe,IdExameFaixa,IdAtleta,DataPagamento,Valor, Observacao, IdFormaPagamento")] ExameFaixaDetalhe ExameFaixaDetalhe)
        {
            if (id != ExameFaixaDetalhe.IdExameFaixaDetalhe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
//                    ExameFaixaDetalhe.ValorTotal = ExameFaixaDetalhe.Quantidade * ExameFaixaDetalhe.ValorUnitario;
                    _context.Update(ExameFaixaDetalhe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(ExameFaixaDetalhe.IdExameFaixaDetalhe))
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
            return View(ExameFaixaDetalhe);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ExameFaixaDetalhe = await _context.ExameFaixaDetalhe.Include(a => a.IdExameNavigation)
                .FirstOrDefaultAsync(m => m.IdExameFaixaDetalhe == id);
            if (ExameFaixaDetalhe == null)
            {
                return NotFound();
            }

            return View(ExameFaixaDetalhe);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ExameFaixaDetalhe = await _context.ExameFaixaDetalhe.FindAsync(id);
            _context.ExameFaixaDetalhe.Remove(ExameFaixaDetalhe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.ExameFaixaDetalhe.Any(e => e.IdExameFaixaDetalhe == id);
        }
    }
}
