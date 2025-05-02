using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class Professor
    {
        public Professor()
        {
            ProfessorModalidade = new HashSet<ProfessorModalidade>();
        }

        public int IdProfessor { get; set; }
        [DisplayName("Nome")]
        public string NomeProfessor { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<ProfessorModalidade> ProfessorModalidade { get; set; }
        public virtual ICollection<Presenca> Presenca { get; set; }
        public virtual ICollection<Mensalidade> Mensalidade { get; set; }

    }
}
