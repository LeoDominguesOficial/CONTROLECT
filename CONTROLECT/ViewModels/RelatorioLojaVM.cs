using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CONTROLECT.ViewModels
{
    public class RelatorioLojaVM
    {
        [DisplayName("Item")]
        public string NomeItem { get; set; }

        [DisplayName("Valor Unitário")]
        public double ValorUnitario { get; set; }

        [DisplayName("Quantidade")]
        public int Quantidade { get; set; }

        [DisplayName("Valor Total")]
        public double ValorTotal { get; set; }

        [DisplayName("Data Venda")]
        public DateTime DataVenda { get; set; }

        [DisplayName("Data Hora Venda")]
        public DateTime DataHoraVenda { get; set; }

        [DisplayName("Comprador")]
        public string Comprador{ get; set; }

        [DisplayName("Quitado")]
        public bool Quitado { get; set; }

        [DisplayName("Forma Pagamento")]
        public string NomeFormaPagamento { get; set; }


    }
}
