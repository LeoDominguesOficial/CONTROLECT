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
    public class ItemDespesasController : Controller
    {
        private readonly AppDbContext _context;

        public ItemDespesasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(int meses, int ano)
        {
            int mesAtual = DateTime.Now.Month;
            ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);

            var despesas = from desp in _context.ItemDespesa.Include(a => a.IdDespesaNavigation).OrderByDescending(a => a.DataPagamento) select desp;


            if (meses > 0)
            {
                despesas = despesas.Where(s => s.DataPagamento.Month == meses);
            }

            if (ano > 0)
            {
                despesas = despesas.Where(s => s.DataPagamento.Year == ano);
            }


            return View(await despesas.OrderByDescending(a=>a.IdItemDespesa).AsNoTracking().ToListAsync());

            //return View(await _context.ItemDespesa.Include(a=>a.IdDespesaNavigation).OrderByDescending(a=>a.DataPagamento).ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ItemDespesa = await _context.ItemDespesa.Include(a=>a.IdDespesaNavigation)
                .FirstOrDefaultAsync(m => m.IdItemDespesa == id);
            if (ItemDespesa == null)
            {
                return NotFound();
            }

            return View(ItemDespesa);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["IdDespesa"] = new SelectList(_context.Despesa.Where(a => a.Ativo == true).OrderBy(a => a.NomeDespesa), "IdDespesa", "NomeDespesa");

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdItemDespesa,IdDespesa,Quantidade,ValorUnitario,ValorTotal,DataPagamento")] ItemDespesa ItemDespesa)
        {
            if (ModelState.IsValid)
            {
                ItemDespesa.ValorTotal = ItemDespesa.ValorUnitario * ItemDespesa.Quantidade;

                _context.Add(ItemDespesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ItemDespesa);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ItemDespesa = await _context.ItemDespesa.FindAsync(id);


            if (ItemDespesa == null)
            {
                return NotFound();
            }

            ItemDespesa.ValorTotal = ItemDespesa.ValorUnitario * ItemDespesa.Quantidade;
            
            ViewData["IdDespesa"] = new SelectList(_context.Despesa, "IdDespesa", "NomeDespesa", ItemDespesa.IdDespesa);

            return View(ItemDespesa);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdItemDespesa,IdDespesa,Quantidade,ValorUnitario,ValorTotal,DataPagamento")] ItemDespesa ItemDespesa)
        {
            if (id != ItemDespesa.IdItemDespesa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ItemDespesa.ValorTotal = ItemDespesa.Quantidade * ItemDespesa.ValorUnitario;
                    _context.Update(ItemDespesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(ItemDespesa.IdItemDespesa))
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
            return View(ItemDespesa);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ItemDespesa = await _context.ItemDespesa.Include(a => a.IdDespesaNavigation)
                .FirstOrDefaultAsync(m => m.IdItemDespesa == id);
            if (ItemDespesa == null)
            {
                return NotFound();
            }

            return View(ItemDespesa);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ItemDespesa = await _context.ItemDespesa.FindAsync(id);
            _context.ItemDespesa.Remove(ItemDespesa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.ItemDespesa.Any(e => e.IdItemDespesa == id);
        }
    }
}
