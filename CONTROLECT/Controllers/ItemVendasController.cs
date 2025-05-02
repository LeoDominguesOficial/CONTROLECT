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
    public class ItemVendasController : Controller
    {
        private readonly AppDbContext _context;

        public ItemVendasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ItemVendas
        public async Task<IActionResult> Index(int ItemId, DateTime dataInicial, DateTime dataFinal)
        {
            ViewData["ItemId"] = new SelectList(_context.Item.Where(a => a.Ativo == true && a.Loja == true).OrderBy(a => a.NomeItem), "IdItem", "NomeItem");

            var vendas = _context.ItemVenda.Where(a => a.Lanchonete == true)
                .Include(a=>a.IdFormaPagamentoNavigation)
                .Include(a=>a.IdItemNavigation)
                .Where(a=>a.DataVenda >= dataInicial && a.DataVenda <= dataFinal)
                .OrderByDescending(a => a.IdItemVenda).ToListAsync();

            return View(await vendas);
        }

        public async Task<IActionResult> IndexLanchonete()
        {
            return View(await _context.ItemVenda.Where(a => a.Lanchonete == true).OrderByDescending(a => a.IdItemVenda).ToListAsync());
        }

        // GET: ItemVendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Item = await _context.ItemVenda
                .FirstOrDefaultAsync(m => m.IdItemVenda == id);
            if (Item == null)
            {
                return NotFound();
            }

            return View(Item);
        }

        // GET: ItemVendas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdItemVenda,IdItem,Quantidade,ValorUnitario,ValorTotal,DataVenda,Comprador,Quitado,DataHoraVenda,IdFormaPagamento,Loja,Lanchonete")] ItemVenda ItemVenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ItemVenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ItemVenda);
        }

        // GET: ItemVendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ItemVenda = await _context.ItemVenda.FindAsync(id);
            if (ItemVenda == null)
            {
                return NotFound();
            }
            return View(ItemVenda);
        }

        // POST: ItemVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdItemVenda,IdItem,Quantidade,ValorUnitario,ValorTotal,DataVenda,Comprador,Quitado,DataHoraVenda,IdFormaPagamento,Loja,Lanchonete")] ItemVenda ItemVenda)
        {
            if (id != ItemVenda.IdItemVenda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ItemVenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(ItemVenda.IdItemVenda))
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
            return View(ItemVenda);
        }

        // GET: ItemVendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ItemVenda = await _context.ItemVenda
                .FirstOrDefaultAsync(m => m.IdItemVenda == id);
            if (ItemVenda == null)
            {
                return NotFound();
            }

            return View(ItemVenda);
        }

        // POST: ItemVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ItemVenda = await _context.ItemVenda.FindAsync(id);
            _context.ItemVenda.Remove(ItemVenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.ItemVenda.Any(e => e.IdItem == id);
        }
    }
}
