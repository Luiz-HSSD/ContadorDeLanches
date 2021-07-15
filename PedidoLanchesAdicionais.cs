using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContadorDeLanches
{
    public class PedidoLanchesAdicionais
    {
        [Key, Column(Order = 1)]
        public int IdLanche { get; set; }

        [Key, Column(Order = 2)]
        public int IdPedido { get; set; }
        [Key, Column(Order = 3)]
        public int IdItem { get; set; }
        [Key, Column(Order = 4)]
        public int Id { get; set; }
        public int IdAdicional { get; set; }
        public string Nome { get; set; }
        [NotMapped]
        public double Preco { get; set; }
    }
}
