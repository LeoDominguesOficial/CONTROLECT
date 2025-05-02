//using System.Web;
//using System.Web.Mvc;
//using CONTROLECT.Models;
//using CONTROLECT.ViewModels;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONTROLECT.Models;
using CONTROLECT.ViewModels;

namespace CONTROLECT.Controllers
{
    public class RelatorioLanchonetesController : Controller
    {
        private readonly AppDbContext _context;

        public RelatorioLanchonetesController(AppDbContext context)
        {
            _context = context;
        }


        //[AcceptVerbs(HttpVerbs.Get)]

        //public ActionResult Index()
        public IActionResult Index(string sortOrder, int idItem, DateTime dataInicial, DateTime dataFinal, int? pagina)
        {

            ViewData["ItemId"] = new SelectList(_context.Item.Where(a => a.Ativo == true && a.Lanchonete == true).OrderBy(a => a.NomeItem), "IdItem", "NomeItem");
            //ViewData["FormaPagamentoId"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
            //ViewData["ProfessorId"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");

            decimal valorTotalRecebido = 0;


            List<RelatorioLanchoneteVM> RelatorioLanchoneteVMList = new List<RelatorioLanchoneteVM>();


            var RelatorioLanchoneteList = (from itemVendas in _context.ItemVenda
                                     join item in _context.Item on itemVendas.IdItem equals item.IdItem
                                     join forma in _context.Formapagamento on itemVendas.IdFormaPagamento equals forma.IdFormaPagamento                                            
                                     where itemVendas.Lanchonete == true 
                                     select new
                                     {
                                         item.IdItem,
                                         item.NomeItem,
                                         itemVendas.ValorUnitario,
                                         itemVendas.Quantidade,
                                         itemVendas.ValorTotal,
                                         itemVendas.DataVenda,
                                         itemVendas.DataHoraVenda,
                                         itemVendas.Comprador,
                                         itemVendas.Quitado,
                                         forma.NomeFormaPagamento
                                     }).OrderByDescending(a => a.DataVenda).ToList();


            if (idItem > 0)
            {
                RelatorioLanchoneteList = RelatorioLanchoneteList.Where(s => s.IdItem == idItem).ToList();
            }

            if (dataInicial != DateTime.MinValue.Date)
            {
                RelatorioLanchoneteList = RelatorioLanchoneteList.Where(s => s.DataVenda.Date >= dataInicial.Date).ToList();
            }

            if (dataFinal != DateTime.MinValue.Date)
            {
                RelatorioLanchoneteList = RelatorioLanchoneteList.Where(s => s.DataVenda.Date <= dataFinal.Date).ToList();
            }


            //if (professorId > 0)
            //{
            //    RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdProfessor == professorId).ToList();
            //}

            //if (formapagamentoId > 0)
            //{
            //    RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdFormaPagamento == formapagamentoId).ToList();
            //}


            //if (mes > 0)
            //{
            //    RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdMes == mes).ToList();
            //}
            //else
            //{
            //    RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdMes == DateTime.Now.Month).ToList();
            //}

            //if (ano > 0)
            //{
            //    RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.AnoCorrespondente == ano).ToList();
            //}
            //else
            //{
            //    RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.AnoCorrespondente == DateTime.Now.Year).ToList();
            //}


            foreach (var item in RelatorioLanchoneteList)
            {
                RelatorioLanchoneteVM objrel = new RelatorioLanchoneteVM(); // ViewModel

                objrel.NomeItem = item.NomeItem;
                objrel.ValorUnitario = item.ValorUnitario;
                objrel.Quantidade = item.Quantidade;
                objrel.ValorTotal = item.ValorTotal;
                objrel.DataVenda = item.DataVenda;
                objrel.Comprador = item.Comprador;
                objrel.Quitado = item.Quitado;
                objrel.NomeFormaPagamento = item.NomeFormaPagamento;

                RelatorioLanchoneteVMList.Add(objrel);

                valorTotalRecebido = ((decimal)valorTotalRecebido) + ((decimal)objrel.ValorTotal);
            }

            if ((dataInicial != DateTime.MinValue) && (dataFinal != DateTime.MinValue))
            {
                ViewData["PeriodoRelatorio"] = "De: " + dataInicial.Date.Day + "/" + dataInicial.Date.Month.ToString("00") + "/" + dataInicial.Date.Year + " Até " + dataFinal.Date.Day + "/" + dataFinal.Date.Month.ToString("00") + "/" + dataFinal.Date.Year;
            }


            ViewData["ValorTotalRecebido"] = valorTotalRecebido;

            return View(RelatorioLanchoneteVMList);
            //return View();

        }

        //public IActionResult IndexSintetico(string sortOrder, int idItem, DateTime dataInicial, DateTime dataFinal, int? pagina)
        //{

        //    ViewData["ItemId"] = new SelectList(_context.Item.Where(a => a.Ativo == true && a.Lanchonete == true).OrderBy(a => a.NomeItem), "IdItem", "NomeItem");

        //    //ViewData["ModalidadeId"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
        //    //ViewData["ProfessorId"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");

        //    //CarregarListaMeses();

        //    List<RelatorioLanchoneteVM> RelatorioLanchoneteVMList = new List<RelatorioLanchoneteVM>();




        //    var RelatorioLanchoneteList = (from itemVendas in _context.ItemVenda
        //                             join item in _context.Item on itemVendas.IdItem equals item.IdItem
        //                             join forma in _context.Formapagamento on itemVendas.IdFormaPagamento equals forma.IdFormaPagamento                                            //where item.AnoCorrespondente.Equals(ano) && mens.IdMes == mes
        //                             where itemVendas.Lanchonete == true && item.Lanchonete == true
        //                             select new
        //                             {
        //                                 item.IdItem,
        //                                 item.NomeItem,
        //                                 itemVendas.ValorUnitario,
        //                                 itemVendas.Quantidade,
        //                                 itemVendas.ValorTotal,
        //                                 itemVendas.DataVenda,
        //                                 itemVendas.DataHoraVenda,
        //                                 itemVendas.Comprador,
        //                                 itemVendas.Quitado,
        //                                 forma.NomeFormaPagamento
        //                             }).OrderByDescending(a => a.DataVenda).ToList();


        //    if (idItem > 0)
        //    {
        //        RelatorioLanchoneteList = RelatorioLanchoneteList.Where(s => s.IdItem == idItem).ToList();
        //    }

        //    if (dataInicial != DateTime.MinValue.Date)
        //    {
        //        RelatorioLanchoneteList = RelatorioLanchoneteList.Where(s => s.DataVenda.Date >= dataInicial.Date).ToList();
        //    }

        //    if (dataFinal != DateTime.MinValue.Date)
        //    {
        //        RelatorioLanchoneteList = RelatorioLanchoneteList.Where(s => s.DataVenda.Date <= dataFinal.Date).ToList();
        //    }

        //    var RelatorioSintetico = RelatorioLanchoneteList.GroupBy(r => r.NomeItem).Select(cl => new RelatorioMensalidadeVM
        //    {
        //        NomeProfessor = cl.First().Nomeit,
        //        NomeModalidade = cl.First().NomeModalidade,
        //        Valor = cl.Sum(c => c.Valor),
        //        ValorRepasseProfessor = cl.Sum(c => c.Valor * c.PercentualProfessor / 100),
        //        ValorCT = cl.Sum(c => (c.Valor) - (c.Valor * c.PercentualProfessor / 100))
        //    }).ToList();

        //    return View(RelatorioSintetico);
        //}


    }

}