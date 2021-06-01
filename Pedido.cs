using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContadorDeLanches
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public int IdCliente { get; set; }
        public DateTime Chegada { get; set; }
        public bool ParaViagem { get; set; }
        public int status { get; set; }
    }
}
