using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContadorDeLanches
{
    public class DBInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LancheContexto>
    {
        protected override void Seed(LancheContexto context)
        {
            var lanches = new List<Lanche>
            {
                new Lanche(){Id=1, Nome="x-burger" },
                new Lanche(){Id=2, Nome="x-salada" },
                new Lanche(){Id=3, Nome="x-bacon" },
                new Lanche(){Id=4, Nome="x-frango" },
                new Lanche(){Id=5, Nome="x-tudo" },
                new Lanche(){Id=6, Nome="brutos" },
                new Lanche(){Id=7, Nome="x-calabreza" },
                new Lanche(){Id=8, Nome="x-egg" },
                new Lanche(){Id=9, Nome="x-burger" },
                new Lanche(){Id=10, Nome="x-burger" },
            };
            lanches.ForEach(s => context.Lanches.Add(s));
            context.SaveChanges();
            //context.SaveChanges();
        }
    }
}
