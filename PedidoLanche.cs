using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContadorDeLanches
{
    public class PedidoLanche
    {
        [Key, Column(Order = 1)]
        public int IdLanche { get; set; }

        [Key, Column(Order = 2)]
        public int IdPedido { get; set; }
        public string PontoCarne { get; set; }
        public string Adicionais { get; set; }
        public string Remover { get; set; }
        [NotMapped]
        public  string LancheNome { get; set; }

    }
}
