using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CONTROLECT.Models
{
    public partial class ItemVenda
    {
        public int IdItemVenda { get; set; }
        public int IdItem { get; set; }
        [DisplayName("Quantidade")]
        public int Quantidade{ get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal (10,2)")]
        public double ValorUnitario { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal (10,2)")]
        public double ValorTotal { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataVenda { get; set; }

        public string Comprador { get; set; }

        public bool Quitado { get; set; }

        public DateTime DataHoraVenda { get; set; }

        public int IdFormaPagamento { get; set; }

        public bool Loja { get; set; }

        public bool Lanchonete { get; set; }


        [DisplayName("Venda")]
        public virtual Item IdItemNavigation { get; set; }
        public virtual FormaPagamento IdFormaPagamentoNavigation { get; set; }


    }
}
