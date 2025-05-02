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
    public class RelatorioDespesasController : Controller
    {
        private readonly AppDbContext _context;

        public RelatorioDespesasController(AppDbContext context)
        {
            _context = context;
        }



        private void CarregarListaMeses()
        {
            ViewData["Mes"] = new[]
{
                new SelectListItem(){ Value = "1", Text = "Janeiro"},
                new SelectListItem(){ Value = "2", Text = "Fevereiro"},
                new SelectListItem(){ Value = "3", Text = "Março"},
                new SelectListItem(){ Value = "4", Text = "Abril"},
                new SelectListItem(){ Value = "5", Text = "Maio"},
                new SelectListItem(){ Value = "6", Text = "Junho"},
                new SelectListItem(){ Value = "7", Text = "Julho"},
                new SelectListItem(){ Value = "8", Text = "Agosto"},
                new SelectListItem(){ Value = "9", Text = "Setembro"},
                new SelectListItem(){ Value = "10", Text = "Outubro"},
                new SelectListItem(){ Value = "11", Text = "Novembro"},
                new SelectListItem(){ Value = "12", Text = "Dezembro"}
            };
        }



        //[AcceptVerbs(HttpVerbs.Get)]

        //public ActionResult Index()
        public IActionResult Index(string sortOrder, string nomeAtleta, int modalidadeId, int professorId, int formapagamentoId,  int mes, int ano, int? pagina)
        {

            ViewData["ModalidadeId"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["FormaPagamentoId"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
            ViewData["ProfessorId"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");

            CarregarListaMeses();
            decimal valorTotalRecebido = 0;
            decimal valorTotalRepasse = 0;


            List<RelatorioMensalidadeVM> RelatorioMensalidadeVMList = new List<RelatorioMensalidadeVM>();


            var RelatorioMensalidadeList = (from mens in _context.Mensalidade
                                            join atle in _context.Atleta on mens.IdAtleta equals atle.IdAtleta
                                            join moda in _context.Modalidade on mens.IdModalidade equals moda.IdModalidade
                                            join prof in _context.Professor on mens.IdProfessor equals prof.IdProfessor
                                            where mens.AnoCorrespondente.Equals(ano) && mens.IdMes == mes
                                            orderby mens.DataSistema descending
                                            select new {    atle.NomeCompleto, moda.NomeModalidade, moda.PercentualProfessor, prof.NomeProfessor, mens.Valor, 
                                                            mens.ValorRepasseProfessor, mens.QuitadoProfessor, mens.DataPagamento, mens.NumeroRecibo, 
                                                            mens.IdAtleta, mens.IdProfessor, mens.IdModalidade, mens.IdMes,
                                                            mens.AnoCorrespondente, mens.IdFormaPagamento}).ToList();


            if (!String.IsNullOrEmpty(nomeAtleta))
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.NomeCompleto.ToUpper().Contains(nomeAtleta.ToUpper().ToString())).ToList();
            }

            if (modalidadeId > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdModalidade == modalidadeId).ToList();
            }

            if (professorId > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdProfessor == professorId).ToList();
            }

            if (formapagamentoId > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdFormaPagamento == formapagamentoId).ToList();
            }


            if (mes > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdMes == mes).ToList();
            }
            else
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdMes == DateTime.Now.Month).ToList();
            }

            if (ano > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.AnoCorrespondente == ano).ToList();
            }
            else
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.AnoCorrespondente == DateTime.Now.Year).ToList();
            }


            foreach (var item in RelatorioMensalidadeList)
            {
                RelatorioMensalidadeVM objrel = new RelatorioMensalidadeVM(); // ViewModel

                objrel.NomeCompleto = item.NomeCompleto;
                objrel.NomeModalidade = item.NomeModalidade;
                objrel.Valor = item.Valor;
                objrel.ValorRepasseProfessor = (item.Valor / item.PercentualProfessor);
                objrel.QuitadoProfessor = item.QuitadoProfessor;
                objrel.DataPagamento = item.DataPagamento;
                objrel.NumeroRecibo = item.NumeroRecibo;
                objrel.NomeProfessor = item.NomeProfessor;

                RelatorioMensalidadeVMList.Add(objrel);

                valorTotalRecebido = valorTotalRecebido + objrel.Valor;
                valorTotalRepasse = valorTotalRepasse + objrel.ValorRepasseProfessor;
            }

            ViewData["ValorTotalRecebido"] = valorTotalRecebido;
            ViewData["ValorTotalRepasse"] = Convert.ToDouble(valorTotalRepasse);

            return View(RelatorioMensalidadeVMList);
            //return View();

        }

        public IActionResult IndexSintetico(string sortOrder, int modalidadeId, int professorId, int mes, int ano, int? pagina)
        {

            ViewData["ModalidadeId"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["ProfessorId"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");

            CarregarListaMeses();

            List<RelatorioMensalidadeVM> RelatorioMensalidadeVMList = new List<RelatorioMensalidadeVM>();


            var RelatorioMensalidadeList = (from mens in _context.Mensalidade
                                            join atle in _context.Atleta on mens.IdAtleta equals atle.IdAtleta
                                            join moda in _context.Modalidade on mens.IdModalidade equals moda.IdModalidade
                                            join prof in _context.Professor on mens.IdProfessor equals prof.IdProfessor
                                            orderby mens.DataSistema descending
                                            select new
                                            {
                                                moda.NomeModalidade,
                                                moda.PercentualProfessor,
                                                prof.NomeProfessor,
                                                mens.Valor,
                                                mens.ValorRepasseProfessor,
                                                mens.IdProfessor,
                                                mens.IdModalidade,
                                                mens.IdMes,
                                                mens.AnoCorrespondente
                                            }).ToList();

            if (modalidadeId > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdModalidade == modalidadeId).ToList();
            }

            if (professorId > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdProfessor == professorId).ToList();
            }

            if (mes > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.IdMes == mes).ToList();
            }

            if (ano > 0)
            {
                RelatorioMensalidadeList = RelatorioMensalidadeList.Where(s => s.AnoCorrespondente == ano).ToList();
            }

            var RelatorioSintetico = RelatorioMensalidadeList.GroupBy(r => r.IdProfessor).Select(cl => new RelatorioMensalidadeVM
            {
                NomeProfessor = cl.First().NomeProfessor,
                NomeModalidade = cl.First().NomeModalidade,
                Valor = cl.Sum(c => c.Valor),
                ValorRepasseProfessor = cl.Sum(c => c.Valor / c.PercentualProfessor),
                ValorCT = cl.Sum(c => (c.Valor) - (c.Valor / c.PercentualProfessor))
            }).ToList();

            return View(RelatorioSintetico);
        }


    }

}