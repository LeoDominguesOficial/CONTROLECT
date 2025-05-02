using System;
using System.Collections.Generic;

namespace CONTROLECT.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
    }
}
