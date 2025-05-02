using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CONTROLECT.Models
{
    public partial class Mensalidade
    {
        public int IdMensalidade { get; set; }
        [DisplayName("Atleta")]
        public int IdAtleta { get; set; }
        [DisplayName("Modalidade")]
        public int IdModalidade { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data Pagamento")]
        public DateTime DataPagamento { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal (10,2)")]
        public decimal Valor { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }
        [DisplayName("Mês Referente")]
        public int? IdMes { get; set; }
        [DisplayName("Ano Referente")]
        public int? AnoCorrespondente { get; set; }

        [DisplayName("Nº Recibo")]
        public int? NumeroRecibo { get; set; }
        [DisplayName("Data Sistema")]
        public DateTime DataSistema { get; set; }

        [DisplayName("Forma de Pagamento")]
        public int? IdFormaPagamento { get; set; }

        [DisplayName("Professor")]
        public int? IdProfessor { get; set; }

        [DisplayName("Valor repassado ao Professor ?")]
        public bool QuitadoProfessor { get; set; }

        [DisplayName("Valor do Professor")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal ValorRepasseProfessor { get; set; }


        [DisplayName("Atleta")]
        public virtual Atleta IdAtletaNavigation { get; set; }

        [DisplayName("Modalidade")]
        public virtual Modalidade IdModalidadeNavigation { get; set; }

        //[DisplayName("Mês Referente")]
        //public virtual Mes IdMesNavigation { get; set; }

        [DisplayName("Forma de Pagamento")]
        public virtual FormaPagamento IdFormaPagamentoNavigation { get; set; }

        [DisplayName("Professor")]
        public virtual Professor IdProfessorNavigation { get; set; }

        [DisplayName("Mes")]
        public virtual Mes IdMesNavigation { get; set; }


    }

}
