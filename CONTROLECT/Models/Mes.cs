using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class Mes
    {
        public int IdMes { get; set; }
        [DisplayName("Descrição")]
        public string NomeMes { get; set; }

        public virtual ICollection<Mensalidade> Mensalidade { get; set; }

    }
}
