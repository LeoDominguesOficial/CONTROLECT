using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CONTROLECT.Models
{
    public partial class Faixa
    {


        public int IdFaixa { get; set; }
        [DisplayName("Faixa")]
        public string NomeFaixa { get; set; }
    }
}
