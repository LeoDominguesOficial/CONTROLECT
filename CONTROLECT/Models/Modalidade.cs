using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class Modalidade
    {
        public Modalidade()
        {
            AtletaModalidade = new HashSet<AtletaModalidade>();
            Mensalidade = new HashSet<Mensalidade>();
            ProfessorModalidade = new HashSet<ProfessorModalidade>();
            ExameFaixa = new HashSet<ExameFaixa>();
        }

        public int IdModalidade { get; set; }
        [DisplayName("Nome")]
        public string NomeModalidade { get; set; }
        public bool Ativo { get; set; }
        public int PercentualProfessor { get; set; }
        public virtual ICollection<AtletaModalidade> AtletaModalidade { get; set; }
        public virtual ICollection<Mensalidade> Mensalidade { get; set; }
        public virtual ICollection<ProfessorModalidade> ProfessorModalidade { get; set; }
        public virtual ICollection<Presenca> Presenca { get; set; }
        public virtual ICollection<ExameFaixa> ExameFaixa{ get; set; }



    }
}
