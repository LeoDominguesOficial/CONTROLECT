using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class FormaPagamento
    {
        public int IdFormaPagamento { get; set; }
        [DisplayName("Descrição")]
        public string NomeFormaPagamento { get; set; }

        public virtual ICollection<Mensalidade> Mensalidade { get; set; }
        public virtual ICollection<ItemVenda> ItemVenda { get; set; }

        public virtual ICollection<ExameFaixaDetalhe> ExameFaixaDetalhe { get; set; }

    }
}
