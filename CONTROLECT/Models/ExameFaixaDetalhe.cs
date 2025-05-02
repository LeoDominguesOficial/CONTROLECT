using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CONTROLECT.Models
{
    public partial class ExameFaixaDetalhe
    {
        public int IdExameFaixaDetalhe { get; set; }
        public int IdExameFaixa { get; set; }

        public int IdAtleta { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataPagamento { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal (10,2)")]
        public double Valor { get; set; }

        public string Observacao { get; set; }

        public int IdFormaPagamento { get; set; }


        [DisplayName("ExameFaixa")]
        public virtual ExameFaixa IdExameNavigation { get; set; }

        public virtual Atleta IdAtletaNavigation { get; set; }

        public virtual FormaPagamento IdFormaPagamentoNavigation { get; set; }

    }
}
