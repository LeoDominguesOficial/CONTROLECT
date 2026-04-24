using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONTROLECT.Models;
using System.Web;
using PagedList;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Drawing;
using SQLitePCL;
using System.Text;
using System.Reflection.Metadata;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace CONTROLECT.Controllers
{
    public class MensalidadesController : Controller
    {
        private readonly AppDbContext _context;

        public MensalidadesController(AppDbContext context)
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

            var retorno = Json(new SelectList(listaAtletas.OrderBy(a=>a.NomeCompleto), "IdAtleta", "NomeCompleto"));

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


        //private void CarregarListaMeses()
        //{
        //    var meses = new[]
        //    {
        //        new SelectListItem(){ Value = "1", Text = "Janeiro"},
        //        new SelectListItem(){ Value = "2", Text = "Fevereiro"},
        //        new SelectListItem(){ Value = "3", Text = "Março"},
        //        new SelectListItem(){ Value = "4", Text = "Abril"},
        //        new SelectListItem(){ Value = "5", Text = "Maio"},
        //        new SelectListItem(){ Value = "6", Text = "Junho"},
        //        new SelectListItem(){ Value = "7", Text = "Julho"},
        //        new SelectListItem(){ Value = "8", Text = "Agosto"},
        //        new SelectListItem(){ Value = "9", Text = "Setembro"},
        //        new SelectListItem(){ Value = "10", Text = "Outubro"},
        //        new SelectListItem(){ Value = "11", Text = "Novembro"},
        //        new SelectListItem(){ Value = "12", Text = "Dezembro"}
        //    };

            
        //    ViewData["Meses"] = meses;

        //}


        // GET: Mensalidades
        public IActionResult Index(string sortOrder, string nomeAtleta, int modalidadeId, int professorId, int meses, int ano, int? pagina)
        {
            int mesAtual = DateTime.Now.Month;
            int paginaTamanho = 10;
            int paginaNumero = (pagina ?? 1);

            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a=>a.Ativo == true).OrderBy(a => a.NomeCompleto), "IdAtleta", "NomeCompleto");

            ViewData["ModalidadeId"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");

            ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");

            ViewData["ProfessorId"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");


            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (ano > 0)
            {
                ViewData["Ano"] = ano; 
                //ViewData["Ano"] = System.DateTime.Now.Year;
            }
            //else
            //{
            //    ViewData["Ano"] = ano;
            //}

            ViewData["FiltroAtleta"] = nomeAtleta;
            ViewData["FiltroModalidade"] = modalidadeId;

            //ViewData["Meses"] = meses;
            ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);


            //CarregarListaMeses();

            var mensalidades = from s in _context.Mensalidade.Include(m => m.IdAtletaNavigation)
                               .Include(m => m.IdModalidadeNavigation)
                               .Include(m => m.IdFormaPagamentoNavigation)
                               .Include(m => m.IdProfessorNavigation)
                               .Include(m => m.IdMesNavigation)

                               select s;

            if (!String.IsNullOrEmpty(nomeAtleta))
            {
                mensalidades = mensalidades.Where(s => s.IdAtletaNavigation.PrimeiroNome .Contains(nomeAtleta));
            }

            if (modalidadeId > 0)
            {
                mensalidades = mensalidades.Where(s => s.IdModalidadeNavigation.IdModalidade == modalidadeId);
            }

            if (professorId > 0)
            {
                mensalidades = mensalidades.Where(s => s.IdProfessor == professorId);
            }


            if (meses > 0)
            {
                mensalidades = mensalidades.Where(s => s.IdMes == meses);
            }

            if (ano > 0)
            {
                mensalidades = mensalidades.Where(s => s.AnoCorrespondente == ano);
            }

            ViewData["FiltroModalidade"] = modalidadeId;
            ViewData["FiltroMes"] = meses;
            ViewData["FiltroProfessor"] = professorId;


            switch (sortOrder)
            {
                case "name_desc":
                    mensalidades = mensalidades.OrderByDescending(s => s.IdAtletaNavigation.PrimeiroNome);
                    break;
                case "Date":
                    mensalidades = mensalidades.OrderBy(s => s.IdAtletaNavigation.PrimeiroNome);
                    break;
                case "date_desc":
                    mensalidades = mensalidades.OrderByDescending(s => s.IdAtletaNavigation.PrimeiroNome);
                    break;
                default:
                    mensalidades = mensalidades.OrderByDescending(s => s.DataSistema).ThenBy(s => s.IdModalidadeNavigation.NomeModalidade);
                    break;
            }
            return View(mensalidades.ToPagedList(paginaNumero, paginaTamanho));
            //return View(listaAlunos.ToPagedList(paginaNumero, paginaTamanho));
        }

        // GET: Mensalidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensalidade = await _context.Mensalidade
                .Include(m => m.IdAtletaNavigation)
                .Include(m => m.IdModalidadeNavigation)
                .FirstOrDefaultAsync(m => m.IdMensalidade == id);
            if (mensalidade == null)
            {
                return NotFound();
            }

            return View(mensalidade);
        }

        // GET: Mensalidades/Create
        public IActionResult Create()
        {
            int mesAtual = DateTime.Now.Month;
            //ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a=>a.Ativo == true).OrderBy(a=>a.NomeCompleto), "IdAtleta", "NomeCompleto");
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a=>a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
            //ViewData["IdProfessor"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor");
            ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);

            //CarregarListaMeses();
            return View();
        }


        //// POST: Mensalidades/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdMensalidade,IdAtleta,IdModalidade,DataPagamento,Valor,Observacao,IdMes,NumeroRecibo,IdFormaPagamento,QuitadoProfessor,IdProfessor,ValorRepasseProfessor,AnoCorrespondente")] Mensalidade mensalidade)
        //{
        //    ViewData["identAtleta"] = mensalidade.IdAtleta;

        //    if (ModelState.IsValid)
        //    {
        //        var ultimoRecibo = Convert.ToInt32(_context.Mensalidade.Max(p => p.NumeroRecibo));
        //        mensalidade.NumeroRecibo = ultimoRecibo + 1;
        //        mensalidade.DataSistema = DateTime.Now;
        //        _context.Add(mensalidade);
        //        await _context.SaveChangesAsync();


        //        //Atleta dadosAtleta = _context.Atleta.Where(a => a.IdAtleta == mensalidade.IdAtleta).First();
        //        // EnviarEmail(dadosAtleta.Email, mensalidade.Valor, mensalidade.IdMes.ToString(), dadosAtleta.NomeCompleto);

        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["IdAtleta"] = new SelectList(_context.Atleta, "IdAtleta", "NomeCompleto", mensalidade.IdAtleta);
        //    //ViewData["IdModalidade"] = new SelectList(_context.Modalidade, "IdModalidade", "NomeModalidade", mensalidade.IdModalidade);

        //    return View(mensalidade);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(int[] IdAtleta, int IdMensalidade, int IdModalidade, DateTime DataPagamento, decimal Valor, 
            string Observacao, short IdMes, int NumeroRecibo, int IdFormaPagamento, bool QuitadoProfessor, int IdProfessor, 
            decimal ValorRepasseProfessor, int AnoCorrespondente)
        {
            Mensalidade mensalidade = new Mensalidade();

            int mesAtual = DateTime.Now.Month;
            if (IdAtleta.Count() == 0)
            {
                ViewData["MensagemRetorno"] = "SELECIONE O ATLETA";

                ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
                ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
                ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);
                
                
                return View();
            }
            
            StringBuilder nomesatletas = new StringBuilder();
            var objAtleta = new Atleta();
            var nomeAtleta = string.Empty;

            var listaNome = new List<string>();

           
            foreach (var atleta in IdAtleta)
            {
                mensalidade = new Mensalidade();
                var ultimoRecibo = Convert.ToInt32(_context.Mensalidade.Max(p => p.NumeroRecibo));
                mensalidade.NumeroRecibo = ultimoRecibo + 1;
                mensalidade.DataSistema = DateTime.Now;
                mensalidade.IdAtleta = atleta;
                mensalidade.IdModalidade = IdModalidade;
                mensalidade.DataPagamento = DataPagamento;
                mensalidade.Valor = Valor;
                mensalidade.Observacao = Observacao;
                mensalidade.IdMes = IdMes;

                mensalidade.AnoCorrespondente = AnoCorrespondente;
                mensalidade.Observacao = Observacao;
                mensalidade.IdFormaPagamento = IdFormaPagamento;
                mensalidade.QuitadoProfessor = QuitadoProfessor;
                mensalidade.IdProfessor = IdProfessor;
                mensalidade.ValorRepasseProfessor = ValorRepasseProfessor;

                _context.Add(mensalidade);
                await _context.SaveChangesAsync();

                //Enviar email para o atleta com o comprovanmte
                
                
                
                //EnviarEmail(mensalidade.NumeroRecibo.ToString() + AnoCorrespondente.ToString(), "ldomingues@gmail.com", mensalidade.Valor, IdMes.ToString(), "Teste Usuário");


                objAtleta = _context.Atleta.Where(a => a.IdAtleta.Equals(mensalidade.IdAtleta)).FirstOrDefault();

                listaNome.Add(objAtleta.NomeCompleto);
                nomesatletas.AppendLine(objAtleta.NomeCompleto);
                nomesatletas.AppendLine();
            }


            mesAtual = DateTime.Now.Month;
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true).OrderBy(a => a.NomeModalidade), "IdModalidade", "NomeModalidade");
            ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
            ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes", mesAtual);

            ViewData["MensagemRetorno"] = nomesatletas.ToString();
            ViewData["NomesRetorno"] = listaNome;

            return View();



            //return View(mensalidade);
            //return RedirectToAction(nameof(Index));
        }


        private void EnviarEmail(string Recibo, string emailDestinatario, decimal valor, string mes, string nomeAtleta)
        {
            try
            {
                var descricaoEmail = string.Empty;

                descricaoEmail = "Recebemos de nomeAtleta";

                descricaoEmail.Replace("nomeAtleta", nomeAtleta);


                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Impacto Iraja", "leodomingues.oficial@outlook.com"));
                email.To.Add(new MailboxAddress(nomeAtleta, "ldomingues@gmail.com"));

                email.Subject = "Testing out email sending";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "<b>Hello all the way from the land of C#</b>"
                };

                //var smtp = new MailKit.Net.Smtp.SmtpClient();

                //smtp.UseDefaultCredentials = false;

                //smtp.ConnectAsync("smtp-mail.outlook.com", 587, false);

                //// Note: only needed if the SMTP server requires authentication
                //smtp.AuthenticateAsync(@"leodomingues.oficial@outlook.com.br", @"L&ozinhoJudoca&1");

                //smtp.SendAsync(email);
                //smtp.DisconnectAsync(true);



                using (var smtp1 = new MailKit.Net.Smtp.SmtpClient())
                {


                    smtp1.Connect("smtp.offie365.com", 587, false);
                    //smtp1.Connect("smtp.gmail.com", 587, false);
                    // Note: only needed if the SMTP server requires authentication
                    smtp1.Authenticate(@"leodomingues.oficial@outlook.com", @"L&ozinhoJudoca&1");

                    smtp1.Send(email);
                    smtp1.Disconnect(true);
                }

                //using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                //{
                //    //O ultimo parametro é para usar SSL
                //    emailClient.Connect("smtp.gmail.com", 587, false);

                //    //Remover qualquer funcionalidade OAuth
                //    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                //    emailClient.Authenticate(@"ldomingues@gmail.com", @"L&ozinhoJudoca#10");
                //    emailClient.Send(email);
                //    emailClient.Disconnect(true);
                //}

                //var message = new MailMessage();
                //message.From = new MailAddress("ldomingues@gmal.com");
                //message.Body = "teste de envio de email";

                //using (var emailClient = new System.Net.Mail.SmtpClient())
                //{
                //    //O ultimo parametro é para usar SSL
                //    emailClient.Host = "smtp.gmail.com";
                //    emailClient.Port = 587;
                //    emailClient.EnableSsl = true;
                //    emailClient.UseDefaultCredentials = false;
                //    emailClient.Credentials = new NetworkCredential(@"ldomingues@gmail.com", @"L&ozinhoJudoca#10");
                //    emailClient.Send(message);
                //    emailClient.Dispose();

                //    ////Remover qualquer funcionalidade OAuth
                //    //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                //    //emailClient.Authenticate(@"ldomingues@gmail.com", @"L&ozinhoJudoca#10");
                //    //emailClient.Send(email);
                //    //emailClient.Disconnect(true);
                //}



            }
            catch (Exception ex)
            {

            }
        }

        // GET: Mensalidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensalidade = await _context.Mensalidade.FindAsync(id);
            if (mensalidade == null)
            {
                return NotFound();
            }

            //CarregarListaMeses();
            ViewData["IdFormaPagamento"] = new SelectList(_context.Formapagamento.OrderBy(a => a.IdFormaPagamento), "IdFormaPagamento", "NomeFormaPagamento");
            ViewData["IdProfessor"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor", mensalidade.IdProfessor);
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true), "IdAtleta", "NomeCompleto", mensalidade.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", mensalidade.IdModalidade);
            ViewData["Meses"] = new SelectList(_context.Mes.OrderBy(a => a.IdMes), "IdMes", "NomeMes");

            return View(mensalidade);
        }

        // POST: Mensalidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMensalidade,IdAtleta,IdModalidade,DataPagamento,Valor,Observacao,IdMes,NumeroRecibo,DataSistema,IdFormaPagamento,QuitadoProfessor,IdProfessor,ValorRepasseProfessor,AnoCorrespondente")] Mensalidade mensalidade)
        {
            if (id != mensalidade.IdMensalidade)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mensalidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MensalidadeExists(mensalidade.IdMensalidade))
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
            ViewData["IdAtleta"] = new SelectList(_context.Atleta.Where(a => a.Ativo == true), "IdAtleta", "NomeCompleto", mensalidade.IdAtleta);
            ViewData["IdModalidade"] = new SelectList(_context.Modalidade.Where(a => a.Ativo == true), "IdModalidade", "NomeModalidade", mensalidade.IdModalidade);
            ViewData["IdProfessor"] = new SelectList(_context.Professor.Where(a => a.Ativo == true).OrderBy(a => a.NomeProfessor), "IdProfessor", "NomeProfessor", mensalidade.IdProfessor);

            return View(mensalidade);
        }

        // GET: Mensalidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensalidade = await _context.Mensalidade
                .Include(m => m.IdAtletaNavigation)
                .Include(m => m.IdModalidadeNavigation)
                .FirstOrDefaultAsync(m => m.IdMensalidade == id);
            if (mensalidade == null)
            {
                return NotFound();
            }

            return View(mensalidade);
        }

        // POST: Mensalidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mensalidade = await _context.Mensalidade.FindAsync(id);
            _context.Mensalidade.Remove(mensalidade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public FileContentResult GetFoto(int id)
        //{
        //    var atleta = _context.Atleta.Find(id);
        //    if (atleta?.Foto != null)
        //    {
        //        return File(atleta.Foto, "image/jpeg");
        //    }
        //    return null;
        //}


        private bool MensalidadeExists(int id)
        {
            return _context.Mensalidade.Any(e => e.IdMensalidade == id);
        }
    }
}
