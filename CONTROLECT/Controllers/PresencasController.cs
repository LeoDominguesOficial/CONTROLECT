using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONTROLECT.Models;
using System.Text;
using CONTROLECT.ViewModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text.RegularExpressions;

namespace CONTROLECT.Controllers
{
    public class PresencasController : Controller
    {
        private readonly AppDbContext _context;

        public PresencasController(AppDbContext context)
        {
            _context = context;
        }

        public JsonResult ObterAtletasbyid(int idModalidade)
        {

            //var listaAtletas = new List<Atleta>();
            //listaAtletas.Add(new Atleta { IdAtleta = 0, NomeCompleto = "Selecionar Atleta" });

            var listaAtletas =
                (from m in _context.Modalidade
                 join am in _context.AtletaModalidade
                     on m.IdModalidade equals am.IdModalidade
                 join a in _context.Atleta
                     on am.IdAtleta equals a.IdAtleta
                 where
                    am.IdModalidade == idModalidade && am.Ativo == true

                 select new Atleta
                 {
                     IdAtleta = a.IdAtleta,
                     NomeCompleto = a.NomeCompleto
                 }).ToList();

            var retorno = Json(new SelectList(listaAtletas.OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto"));

            return retorno;
        }

        public JsonResult ObterModalidadesbyid(int idAtleta)
        {

            var listamodalidades =
                (from m in _context.Modalidade
                 join am in _context.AtletaModalidade
                     on m.IdModalidade equals am.IdModalidade
                 join a in _context.Atleta
                     on am.IdAtleta equals a.IdAtleta
                 where am.IdAtleta == idAtleta && am.Ativo == true
                 select new Modalidade
                 {
                     IdModalidade = m.IdModalidade,
                     NomeModalidade = m.NomeModalidade
                 }).OrderBy(a => a.NomeModalidade).ToList();
            return Json(new SelectList(listamodalidades, "IdModalidade", "NomeModalidade"));
        }

        public JsonResult ObterProfessoresbyModalidade(int idModalidade)
        {

            var listaProfessores =
                (from p in _context.Professor
                 join pm in _context.ProfessorModalidade
                     on p.IdProfessor equals pm.IdProfessor
                 join m in _context.Modalidade
                     on pm.IdModalidade equals m.IdModalidade
                 where pm.IdModalidade == idModalidade && p.Ativo == true
                 select new Professor
                 {
                     IdProfessor = p.IdProfessor,
                     NomeProfessor = p.NomeProfessor
                 }).OrderBy(a => a.NomeProfessor).ToList();
            return Json(new SelectList(listaProfessores, "IdProfessor", "NomeProfessor"));
        }


        public JsonResult ObterProfessoresbyid(int idAtleta)
        {

            var listaProfessores =
                (from p in _context.Professor
                 join pm in _context.ProfessorModalidade
                     on p.IdProfessor equals pm.IdProfessor
                 join m in _context.Modalidade
                     on pm.IdModalidade equals m.IdModalidade
                 join am in _context.AtletaModalidade
                     on m.IdModalidade equals am.IdModalidade
                 join a in _context.Atleta
                     on am.IdAtleta equals a.IdAtleta
                 where am.IdAtleta == idAtleta && pm.Ativo == true
                 select new Professor
                 {
                     IdProfessor = p.IdProfessor,
                     NomeProfessor = p.NomeProfessor
                 }).OrderBy(a => a.NomeProfessor).ToList();
            return Json(new SelectList(listaProfessores, "IdProfessor", "NomeProfessor"));
        }


        //public JsonResult ObterModalidadesbyid(int idAtleta)
        //{

        //    var listamodalidades =
        //        (from m in _context.Modalidade
        //         join am in _context.AtletaModalidade
        //             on m.IdModalidade equals am.IdModalidade
        //         join a in _context.Atleta
        //             on am.IdAtleta equals a.IdAtleta
        //         where am.IdAtleta == idAtleta
        //         select new Modalidade
        //         {
        //             IdModalidade = m.IdModalidade,
        //             NomeModalidade = m.NomeModalidade
        //         }).ToList();
        //    return Json(new SelectList(listamodalidades, "IdModalidade", "NomeModalidade"));
        //}

        //public JsonResult ObterProfessoresbyid(int idAtleta)
        //{

        //    var listaProfessores =
        //        (from p in _context.Professor
        //         join pm in _context.ProfessorModalidade
        //             on p.IdProfessor equals pm.IdProfessor
        //         join m in _context.Modalidade
        //             on pm.IdModalidade equals m.IdModalidade
        //         join am in _context.AtletaModalidade
        //             on m.IdModalidade equals am.IdModalidade
        //         join a in _context.Atleta
        //             on am.IdAtleta equals a.IdAtleta
        //         where am.IdAtleta == idAtleta
        //         select new Professor
        //         {
        //             IdProfessor = p.IdProfessor,
        //             NomeProfessor = p.NomeProfessor
        //         }).ToList();
        //    return Json(new SelectList(listaProfessores, "IdProfessor", "NomeProfessor"));
        //}


        // GET: AtletaModalidades
        //public async Task<IActionResult> Index()
        //{
        //    var appDbContext = _context.AtletaModalidade.Include(a => a.IdAtletaNavigation).Include(a => a.IdModalidadeNavigation);
        //    return View(await appDbContext.OrderBy(a=>a.IdModalidadeNavigation.NomeModalidade).ThenBy(a=> a.IdAtletaNavigation.NomeCompleto).ToListAsync());
        //}


        public async Task<IActionResult> Index(string sortOrder, string nomeAtleta, int modalidadeId, int turnoId, int professorId, DateTime dataInicial, DateTime dataFinal)
        {
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a=> a.Ativo == true).OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto");
            ViewData["ModalidadeId"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["ProfessorId"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");
            ViewData["TurnoId"] = new SelectList(_context.Turno.OrderBy(a => a.IdTurno), "IdTurno", "NomeTurno");


            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewData["FiltroAtleta"] = nomeAtleta;
            ViewData["FiltroModalidade"] = modalidadeId;
            ViewData["FiltroProfessor"] = professorId;
            ViewData["FiltroTurno"] = turnoId;

            if (dataInicial == DateTime.MinValue)
            {
                dataInicial = DateTime.Now.Date;
            }

            if (dataFinal == DateTime.MinValue)
            {
                dataFinal = DateTime.Now.Date;
            }


            ViewData["FiltroDataInicial"] = dataInicial.Date.ToString("yyyy-MM-dd");
            ViewData["FiltroDataFinal"] = dataFinal.Date.ToString("yyyy-MM-dd");


            //var presencas = from s in _context.Presenca.Include(m => m.IdAtletaNavigation)
            //                                           .Include(m => m.IdModalidadeNavigation)
            //                                           .Include(m => m.IdTurnoNavigation)
            //                                           .Include(m => m.IdProfessorNavigation)
            //                                           .Where(a => a.DataPresenca >= dataInicial && a.DataPresenca <= dataFinal)
            //                        select s;


            List<RelatorioPresencaVM> RelatorioPresencaVMList = new List<RelatorioPresencaVM>();


            //var sql = new StringBuilder();
            //sql.AppendLine("    SELECT [ULTIMOPAGAMENTO] = ISNULL(MAX(M.DataPagamento), CAST('2025-01-01' AS DATE)), MO.NOMEMODALIDADE, P.DATAPRESENCA, A.NOMECOMPLETO,  PR.NOMEPROFESSOR, T.NOMETURNO,  A.DATACADASTRO");
            //sql.AppendLine("    FROM     ");
            //sql.AppendLine("    PRESENCA P ");

            //sql.AppendLine("    JOIN ATLETA A ON a.IdAtleta = p.IdAtleta ");

            //sql.AppendLine("    LEFT JOIN MENSALIDADE M ON M.IDATLETA = A.IDATLETA ");

            //sql.AppendLine("    JOIN MODALIDADE MO ON MO.IDMODALIDADE = P.IDMODALIDADE ");

            //sql.AppendLine("    JOIN PROFESSOR PR ON PR.IDPROFESSOR = P.IDPROFESSOR ");

            //sql.AppendLine("    JOIN TURNO T ON T.IDTURNO = P.IDTURNO ");
            //sql.AppendLine("    WHERE ");

            //sql.AppendLine("     CAST(P.DataPresenca AS DATE) >= '" + dataInicial.ToString("yyyy/MM/dd") + "' and  CAST(P.DataPresenca AS DATE) <= '" + dataFinal.ToString("yyyy/MM/dd") + "'");
            //sql.AppendLine(" GROUP BY P.DATAPRESENCA, A.NOMECOMPLETO,  PR.NOMEPROFESSOR, T.NOMETURNO, MO.NOMEMODALIDADE, A.DATACADASTRO");


            //var teste = _context.Database.ExecuteSqlCommand(sql.ToString());

            //var RelatorioPresencaList = (from pres in _context.Presenca
            //                                 //join mens in _context.Mensalidade on pres.IdAtleta equals mens.IdAtleta 
            //                             join atle in _context.Atleta on pres.IdAtleta equals atle.IdAtleta
            //                             join moda in _context.Modalidade on pres.IdModalidade equals moda.IdModalidade
            //                             join prof in _context.Professor on pres.IdProfessor equals prof.IdProfessor
            //                             join turn in _context.Turno on pres.IdTurno equals turn.IdTurno
            //                             where ((pres.DataPresenca >= dataInicial) && (pres.DataPresenca <= dataFinal)) 
            //                             //group mens by new { atle.NomeCompleto, prof.NomeProfessor, moda.NomeModalidade, 
            //                             //                    turn.NomeTurno, pres.DataPresenca, atle.DataCadastro,
            //                             //                   moda.IdModalidade, prof.IdProfessor
            //                             //                   } into PresencaAgrupada



            //                             select new RelatorioPresencaVM
            //                             {
            //                                NomeCompleto = PresencaAgrupada.Key.NomeCompleto,
            //                                NomeModalidade = PresencaAgrupada.Key.NomeModalidade,
            //                                NomeProfessor = PresencaAgrupada.Key.NomeProfessor,
            //                                DataPresenca = PresencaAgrupada.Key.DataPresenca,
            //                                NomeTurno = PresencaAgrupada.Key.NomeTurno,
            //                                DataCadastro = PresencaAgrupada.Key.DataCadastro,
            //                                IdModalidade = PresencaAgrupada.Key.IdModalidade,
            //                                IdProfessor = PresencaAgrupada.Key.IdProfessor,
            //                                //UltimoPagamento = PresencaAgrupada.Max(g=> g.DataPagamento)
            //                             };

            var RelatorioPresencaList = (from pres in _context.Presenca
                                             //join mens in _context.Mensalidade on pres.IdAtleta equals mens.IdAtleta 
                                         join atle in _context.Atleta on pres.IdAtleta equals atle.IdAtleta
                                         join moda in _context.Modalidade on pres.IdModalidade equals moda.IdModalidade
                                         join prof in _context.Professor on pres.IdProfessor equals prof.IdProfessor
                                         join turn in _context.Turno on pres.IdTurno equals turn.IdTurno
                                         where ((pres.DataPresenca >= dataInicial) && (pres.DataPresenca <= dataFinal))
                                         //group mens by new { atle.NomeCompleto, prof.NomeProfessor, moda.NomeModalidade, 
                                         //                    turn.NomeTurno, pres.DataPresenca, atle.DataCadastro,
                                         //                   moda.IdModalidade, prof.IdProfessor
                                         //                   } into PresencaAgrupada



                                         select new RelatorioPresencaVM
                                         {
                                             NomeCompleto = atle.NomeCompleto,
                                             NomeModalidade = moda.NomeModalidade,
                                             NomeProfessor = prof.NomeProfessor,
                                             DataPresenca = pres.DataPresenca,
                                             NomeTurno = turn.NomeTurno,
                                             DataCadastro = atle.DataCadastro,
                                             IdModalidade = moda.IdModalidade,
                                             IdProfessor = prof.IdProfessor,
                                             UltimoPagamento = _context.Mensalidade.Where(w => w.IdAtleta.Equals(atle.IdAtleta)).Max(m => m.DataPagamento)
                                         });


            if (!String.IsNullOrEmpty(nomeAtleta))
            {
                RelatorioPresencaList = RelatorioPresencaList.Where(s => s.NomeCompleto.Contains(nomeAtleta));
            }

            if (modalidadeId > 0)
            {
                RelatorioPresencaList = RelatorioPresencaList.Where(s => s.IdModalidade == modalidadeId);
            }

            if (professorId > 0)
            {
                RelatorioPresencaList = RelatorioPresencaList.Where(s => s.IdProfessor == professorId);
            }

            //if (turnoId > 0)
            //{
            //    RelatorioPresencaList = RelatorioPresencaList.Where(s => s.IdTurno == turnoId);
            //}

            RelatorioPresencaList = RelatorioPresencaList.OrderByDescending(s => s.DataPresenca).ThenBy(m => m.NomeModalidade).ThenBy(a => a.NomeCompleto);


            return View(await RelatorioPresencaList.AsNoTracking().ToListAsync());

        }



        // GET: Presencas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _context.Presenca
                .Include(a => a.IdAtletaNavigation)
                .Include(a => a.IdModalidadeNavigation)
                .Include(a => a.IdProfessorNavigation)
                .FirstOrDefaultAsync(m => m.IdPresenca == id);
            if (presenca == null)
            {
                return NotFound();
            }

            return View(presenca);
        }

        // GET: Presencas/Create
        public IActionResult Create()
        {
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");


            //ViewData["IdAtleta"] = new SelectList(_context.Atleta.OrderBy(a=>a.NomeCompleto), "IdAtleta", "NomeCompleto");
            //ViewData["IdModalidade"] = new SelectList(_context.Modalidade.OrderBy(a=> a.NomeModalidade), "IdModalidade", "NomeModalidade");
            //ViewData["IdProfessor"] = new SelectList(_context.Professor.OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");
            ViewData["IdTurno"] = new SelectList(_context.Turno.OrderBy(a => a.NomeTurno), "IdTurno", "NomeTurno");

            return View();
        }

        // POST: Presencas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdPresenca,IdAtleta,IdModalidade,IdProfessor,IdTurno,DataPresenca")] Presenca presenca)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(presenca);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true), "IdAtleta", "NomeCompleto", presenca.IdAtleta);
        //    ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", presenca.IdModalidade);
        //    ViewData["IdProfessor"] = new SelectList(_context.Professor.Where(a => a.Ativo == true), "IdProfessor", "NomeProfessor", presenca.IdProfessor);
        //    ViewData["IdTurno"] = new SelectList(_context.Turno, "IdTurno", "NomeTurno", presenca.IdTurno);


        //    return View(presenca);
        //}


        [HttpPost]
        public async Task<IActionResult> Create(int[] IdAtleta, int IdModalidade, DateTime DataPresenca, 
            int IdTurno, int IdProfessor)
        {
            Presenca presenca = new Presenca();

            //int mesAtual = DateTime.Now.Month;
            if (IdAtleta.Count() == 0)
            {
                ViewData["MensagemRetorno"] = "SELECIONE O ATLETA";

                ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
                ViewData["IdTurno"] = new SelectList(_context.Turno, "IdTurno", "NomeTurno", presenca.IdTurno);

                //ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
                //ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);

                return View();
            }

            StringBuilder nomesatletas = new StringBuilder();
            var objAtleta = new Atleta();
            var nomeAtleta = string.Empty;

            var listaNome = new List<string>();


            foreach (var atleta in IdAtleta)
            {
                presenca = new Presenca();
                //var ultimoRecibo = Convert.ToInt32(_context.Mensalidade.Max(p => p.NumeroRecibo));


                presenca.DataPresenca = DataPresenca;
                presenca.IdProfessor = IdProfessor;
                presenca.IdModalidade = IdModalidade;
                presenca.IdTurno = IdTurno;
                presenca.IdAtleta = atleta;
                
                //mensalidade.IdModalidade = IdModalidade;
                //mensalidade.DataPagamento = DataPagamento;
                //mensalidade.Valor = Valor;
                //mensalidade.Observacao = Observacao;
                //mensalidade.IdMes = IdMes;

                //mensalidade.AnoCorrespondente = AnoCorrespondente;
                //mensalidade.Observacao = Observacao;
                //mensalidade.IdFormaPagamento = IdFormaPagamento;
                //mensalidade.QuitadoProfessor = QuitadoProfessor;
                //mensalidade.IdProfessor = IdProfessor;
                //mensalidade.ValorRepasseProfessor = ValorRepasseProfessor;

                _context.Add(presenca);
                await _context.SaveChangesAsync();

                objAtleta = _context.Atleta.Where(a => a.IdAtleta.Equals(presenca.IdAtleta)).FirstOrDefault();

                listaNome.Add(objAtleta.NomeCompleto);
                nomesatletas.AppendLine(objAtleta.NomeCompleto);
                nomesatletas.AppendLine();
            }


            //mesAtual = DateTime.Now.Month;
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdTurno"] = new SelectList(_context.Turno, "IdTurno", "NomeTurno");

            //ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
            //ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);

            ViewData["MensagemRetorno"] = nomesatletas.ToString();
            ViewData["NomesRetorno"] = listaNome;

            //return View(presenca);
            //return View();

            //return View(mensalidade);
            return RedirectToAction(nameof(Index));
        }






        // GET: Presencas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _context.Presenca.FindAsync(id);
            if (presenca == null)
            {
                return NotFound();
            }
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true), "IdAtleta", "NomeCompleto", presenca.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", presenca.IdModalidade);
            ViewData["IdTurno"] = new SelectList(_context.Turno.OrderBy(a => a.IdTurno), "IdTurno", "NomeTurno");

            return View(presenca);
        }

        // POST: Presencas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPresenca,IdAtleta,IdModalidade,IdProfessor,IdTurno,DataPresenca")] Presenca presenca)
        {
            if (id != presenca.IdPresenca)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presenca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresencaExists(presenca.IdPresenca))
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
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true), "IdAtleta", "NomeCompleto", presenca.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", presenca.IdModalidade);
            ViewData["IdProfessor"] = new SelectList(_context.Professor.Where(a => a.Ativo == true), "IdProfessor", "NomeProfessor", presenca.IdProfessor);
            ViewData["IdTurno"] = new SelectList(_context.Turno, "IdTurno", "NomeTurno", presenca.IdTurno);

            return View(presenca);
        }

        // GET: Presencas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenca = await _context.Presenca
                .Include(a => a.IdAtletaNavigation)
                .Include(a => a.IdModalidadeNavigation)
                .Include(a => a.IdProfessor)
                .FirstOrDefaultAsync(m => m.IdPresenca == id);
            if (presenca == null)
            {
                return NotFound();
            }

            return View(presenca);
        }

        // POST: Presencas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presenca = await _context.Presenca.FindAsync(id);
            _context.Presenca.Remove(presenca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresencaExists(int id)
        {
            return _context.Presenca.Any(e => e.IdPresenca == id);
        }
    }
}
