using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContadorDeLanches
{
    public class LancheContexto : DbContext
    {
        public LancheContexto() : base("LancheDbContext")
        {
            Database.CreateIfNotExists();
        }
        public DbSet<Lanche> Lanches { get; set; }
        public DbSet<LancheDia> LanchesDia { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Pagamento> Pagamento { get; set; }
        public DbSet<PedidoLanche> PedidoLanche { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
