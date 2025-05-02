using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CONTROLECT.Models
{
    public partial class Item
    {


        public int IdItem { get; set; }
        [DisplayName("Item")]
        public string NomeItem { get; set; }
        [DataType(DataType.Currency)]
        public double Valor { get; set; }
        public bool Loja { get; set; }
        public bool Lanchonete { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<ItemVenda> ItemVenda { get; set; }

    }
}
