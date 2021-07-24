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
        public int? IdPagamento1 { get; set; }
        public double ValorPago1 { get; set; }
        public int? IdPagamento2 { get; set; }
        public double? ValorPago2 { get; set; }
        public int? IdPagamento3 { get; set; }
        public double? ValorPago3 { get; set; }
        public DateTime Chegada { get; set; }
        public bool ParaViagem { get; set; }
        public int status { get; set; }
        public double Total { get; set; }
    }
}
