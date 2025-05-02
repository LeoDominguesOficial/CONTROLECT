using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class Despesa
    {
        //public Despesa()
        //{
        //    DespesaModalidade = new HashSet<DespesaModalidade>();
        //}

        public int IdDespesa { get; set; }
        [DisplayName("Nome")]
        public string NomeDespesa { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<ItemDespesa> ItemDespesa { get; set; }

        //public virtual ICollection<Presenca> Presenca { get; set; }
        //public virtual ICollection<Mensalidade> Mensalidade { get; set; }

    }
}
