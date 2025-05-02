using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CONTROLECT.ViewModels
{
    public class RelatorioMensalidadeVM
    {
        [DisplayName("Atleta")]
        public string NomeCompleto { get; set; }
        [DisplayName("Professor")]
        public string NomeProfessor { get; set; }
        [DisplayName("Modalidade")]
        public string NomeModalidade { get; set; }
        [DisplayName("Mensalidade")]
        public decimal Valor { get; set; }
        [DisplayName("Pagamento")]
        [DataType(DataType.Date)]
        public DateTime DataPagamento { get; set; }
        [DisplayName("Repasse")]
        public decimal ValorRepasseProfessor { get; set; }
        [DisplayName("Quitado")]
        public bool QuitadoProfessor { get; set; }
        [DisplayName("Recibo")]
        public int? NumeroRecibo { get; set; }
        public decimal ValorCT { get; set; }

    }
}
