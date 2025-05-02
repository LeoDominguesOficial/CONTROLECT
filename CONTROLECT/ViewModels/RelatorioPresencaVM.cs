using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CONTROLECT.ViewModels
{
    public class RelatorioPresencaVM
    {
        [DisplayName("Atleta")]
        public string NomeCompleto { get; set; }

        [DisplayName("Professor")]
        public string NomeProfessor { get; set; }
        
        [DisplayName("Modalidade")]
        public string NomeModalidade { get; set; }

        [DisplayName("Turno")]
        public string NomeTurno { get; set; }

        [DisplayName("Data Presença")]
        [DataType(DataType.Date)]
        public DateTime DataPresenca { get; set; }

        [DisplayName("Data Pagamento")]
        [DataType(DataType.Date)]
        public DateTime UltimoPagamento { get; set; }

        [DisplayName("Data Cadastro")]
        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Situação")]
        public string Situacao { get; set; }

        public int IdProfessor { get; set; }
        public int IdModalidade { get; set; }


    }
}
