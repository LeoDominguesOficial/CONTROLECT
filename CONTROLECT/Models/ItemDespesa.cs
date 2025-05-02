using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CONTROLECT.Models
{
    public partial class ItemDespesa
    {
        public int IdItemDespesa { get; set; }
        public int IdDespesa { get; set; }
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
        public DateTime DataPagamento { get; set; }

        [DisplayName("Despesa")]
        public virtual Despesa IdDespesaNavigation { get; set; }


    }
}
