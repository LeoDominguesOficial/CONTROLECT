using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CONTROLECT.Models
{
    public partial class ProfessorModalidade
    {
        public int IdProfessorModalidade { get; set; }
        [DisplayName("Professor")]
        public int IdProfessor { get; set; }
        [DisplayName("Modalidade")]
        public int IdModalidade { get; set; }
        public bool Ativo { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data Operação")]
        public DateTime DataOperacao { get; set; }

        [DisplayName("Modalidade")]
        public virtual Modalidade IdModalidadeNavigation { get; set; }
        [DisplayName("Professor")]
        public virtual Professor IdProfessorNavigation { get; set; }
    }
}
