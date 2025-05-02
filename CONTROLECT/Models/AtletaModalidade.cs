using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class AtletaModalidade
    {
        public int IdAtletaModalidade { get; set; }
        [DisplayName("Atleta")]
        public int IdAtleta { get; set; }
        [DisplayName("Modalidade")]
        public int IdModalidade { get; set; }
        public bool Ativo { get; set; }
        [DisplayName("Atividade Principal")]
        public bool AtividadePrincipal { get; set; }
        [DisplayName("Turno")]
        public int IdTurno { get; set; }

        [DisplayName("Atleta")]
        public virtual Atleta IdAtletaNavigation { get; set; }
        [DisplayName("Modalidade")]
        public virtual Modalidade IdModalidadeNavigation { get; set; }

        [DisplayName("Turno")]
        public virtual Turno IdTurnoNavigation { get; set; }

    }
}
