using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class Turno
    {

        public Turno()
        {
            AtletaModalidade = new HashSet<AtletaModalidade>();
        }

        public int IdTurno { get; set; }
        [DisplayName("Descrição")]
        public string NomeTurno { get; set; }

        public virtual ICollection<AtletaModalidade> AtletaModalidade { get; set; }
        public virtual ICollection<Presenca> Presenca { get; set; }



    }
}
