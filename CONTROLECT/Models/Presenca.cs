using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CONTROLECT.Models
{
    public partial class Presenca
    {
        public int IdPresenca { get; set; }
        [DisplayName("Atleta")]
        public int IdAtleta { get; set; }
        [DisplayName("Modalidade")]
        public int IdModalidade { get; set; }
        [DisplayName("Professor")]
        public int IdProfessor { get; set; }
        [DisplayName("Turno")]
        public int IdTurno { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data da Presença")]
        public DateTime DataPresenca { get; set; }
        [DisplayName("Atleta")]
        public virtual Atleta IdAtletaNavigation { get; set; }
        [DisplayName("Modalidade")]
        public virtual Modalidade IdModalidadeNavigation { get; set; }
        //public virtual Modalidade IdTurnoNavigation { get; set; }

        [DisplayName("Professor")]
        public virtual Professor IdProfessorNavigation { get; set; }

        [DisplayName("Turno")]
        public virtual Turno IdTurnoNavigation { get; set; }
    }
}
