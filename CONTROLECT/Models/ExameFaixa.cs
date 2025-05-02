using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CONTROLECT.Models
{
    public partial class ExameFaixa
    {


        public int IdExameFaixa { get; set; }
        [DisplayName("NomeExameFaixa")]
        public string NomeExameFaixa { get; set; }

        [DisplayName("Modalidade")]
        public int IdModalidade { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataInicial { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataFinal { get; set; }

        public bool Ativo { get; set; }


        [DisplayName("Modalidade")]
        public virtual Modalidade IdModalidadeNavigation { get; set; }

        public virtual ICollection<ExameFaixaDetalhe> ExameFaixaDetalhe { get; set; }

    }
}
