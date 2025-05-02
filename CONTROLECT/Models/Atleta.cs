using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CONTROLECT.Models
{
    public partial class Atleta
    {
        public Atleta()
        {
            AtletaModalidade = new HashSet<AtletaModalidade>();
            Mensalidade = new HashSet<Mensalidade>();
            ExameFaixaDetalhe = new HashSet<ExameFaixaDetalhe>();

        }

        public int IdAtleta { get; set; }
        [DisplayName("Primeiro Nome")]
        public string PrimeiroNome { get; set; }
        [DisplayName("Último Nome")]
        public string UltimoNome { get; set; }
        [DisplayName("Nome Completo")]
        public string NomeCompleto { get; set; }
        [DisplayName("Responsável")]
        public string NomeResponsavel { get; set; }
        [MaxLength(15)]

        public string Identidade { get; set; }
        [MaxLength(11)]

        public string Cpf { get; set; }
        [MaxLength(9)]
        public string Telefone { get; set; }
        [MaxLength(2)]
        public string Ddd { get; set; }
        [MaxLength(9)]
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        [DisplayName("Número")]
        public int? Numero { get; set; }
        public string Cep { get; set; }

        public string Apelido { get; set; }

        public bool Ativo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DataCadastro { get; set; }


        public virtual ICollection<AtletaModalidade> AtletaModalidade { get; set; }
        public virtual ICollection<Mensalidade> Mensalidade { get; set; }
        public virtual ICollection<Presenca> Presenca { get; set; }
        public virtual ICollection<ExameFaixaDetalhe> ExameFaixaDetalhe { get; set; }

    }
}
