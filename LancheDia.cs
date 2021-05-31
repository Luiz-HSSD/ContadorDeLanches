using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContadorDeLanches
{
    public class LancheDia
    {
        [Key]
        public long Id { get; set; }
        //[ForeignKey("Lanche")]
        public long IdLanche { get; set; }
        public long Qtd { get; set; }
        public DateTime Dia { get; set; }
    }
}
