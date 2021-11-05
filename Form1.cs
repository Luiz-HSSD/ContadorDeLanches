using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ContadorDeLanches
{
    public partial class Form1 : Form
    {
        internal static LancheContexto contexto = new LancheContexto();
        internal static DateTime DiaNormal = DateTime.Now.Date;
        internal static  PedidoForm Compra = new PedidoForm();
        public static Form1 atual;
        public Form1()
        {

            InitializeComponent();
            var lanches = contexto.Lanches.ToList();
            DiaNormal = contexto.LanchesDia.Max(x => x.Dia);
            var lanchesDia = contexto.LanchesDia.Where(x => x.Dia==DiaNormal).ToList();
            for (int i=0; i<lanches.Count; i++)
            {
                var labellanche = new System.Windows.Forms.Label();
                labellanche.AutoSize = true;
                labellanche.BackColor = System.Drawing.SystemColors.Control;
                labellanche.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanche.ForeColor = System.Drawing.Color.Chocolate;
                labellanche.Location = new System.Drawing.Point(16, 46+(i*42));
                labellanche.Name = "labellanche"+i;
                labellanche.Size = new System.Drawing.Size(87, 32);
                labellanche.TabIndex = 2;
                labellanche.Text = lanches.ElementAt(i).Id + " - "+ lanches.ElementAt(i).Nome;
                var labellanche2 = new System.Windows.Forms.Label();
                labellanche2.AutoSize = true;
                labellanche2.BackColor = System.Drawing.SystemColors.Control;
                labellanche2.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanche2.ForeColor = System.Drawing.Color.LimeGreen;
                labellanche2.Location = new System.Drawing.Point(250, 46+(i*42));
                labellanche2.Name = "labellanchen"+i;
                labellanche2.Size = new System.Drawing.Size(87, 32);
                labellanche2.TabIndex = 2;
                labellanche2.Text = lanchesDia.ElementAt(i).Qtd.ToString();
                this.tabPage1.Controls.Add(labellanche);
                this.tabPage1.Controls.Add(labellanche2);
                atual = this;
            }

            var lanchesmen = contexto.LanchesDia.Where(x => x.Dia.Month == DiaNormal.Month && x.Dia.Year == DiaNormal.Year).ToList().GroupBy(x => x.IdLanche)
                .Select(n => new { Qtd = n.Sum(m => m.Qtd), IdLanche = n.Key });
            double totallanches = 0;
            for (int i = 0; i < lanches.Count; i++)
            {
                var labellanchemen = new System.Windows.Forms.Label();
                labellanchemen.AutoSize = true;
                labellanchemen.BackColor = System.Drawing.SystemColors.Control;
                labellanchemen.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanchemen.ForeColor = System.Drawing.Color.Chocolate;
                labellanchemen.Location = new System.Drawing.Point(16, 46 + (i * 42));
                labellanchemen.Name = "labellanchemen" + i;
                labellanchemen.Size = new System.Drawing.Size(87, 32);
                labellanchemen.TabIndex = 2;
                labellanchemen.Text = lanches.ElementAt(i).Id + " - " + lanches.ElementAt(i).Nome;
                var labellanchemen2 = new System.Windows.Forms.Label();
                labellanchemen2.AutoSize = true;
                labellanchemen2.BackColor = System.Drawing.SystemColors.Control;
                labellanchemen2.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanchemen2.ForeColor = System.Drawing.Color.LimeGreen;
                labellanchemen2.Location = new System.Drawing.Point(250, 46 + (i * 42));
                labellanchemen2.Name = "labellanchenmen" + i;
                labellanchemen2.Size = new System.Drawing.Size(87, 32);
                labellanchemen2.TabIndex = 2;
                var tot = lanchesmen.FirstOrDefault(x => x.IdLanche == lanches.ElementAt(i).Id).Qtd;
                totallanches += tot;
                labellanchemen2.Text = tot.ToString();
                this.tabPage2.Controls.Add(labellanchemen);
                this.tabPage2.Controls.Add(labellanchemen2);
                atual = this;
            }
            label2.Text = totallanches.ToString();
            var hoje = DiaNormal;
            var pags = contexto.Pagamento.ToList();
            foreach (var pag in pags)
            {
                var bal = Form1.contexto.Balanco.FirstOrDefault(x => x.IdPagamento == pag.Id && x.Dia == hoje);
                if (bal == null)
                {
                    bal = contexto.Balanco.Add(new Balanco() { Total = 0, IdPagamento = pag.Id, Dia = hoje });
                }

            }
            contexto.SaveChanges();

            pags = contexto.Pagamento.ToList();
            var bals = contexto.Balanco.Where(x => x.Dia == hoje).ToList();
            var labellanchebalsti = new System.Windows.Forms.Label();
            labellanchebalsti.AutoSize = true;
            labellanchebalsti.BackColor = System.Drawing.SystemColors.Control;
            labellanchebalsti.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labellanchebalsti.ForeColor = System.Drawing.Color.Chocolate;
            labellanchebalsti.Location = new System.Drawing.Point(500, 56);
            labellanchebalsti.Name = "balslancheti";
            labellanchebalsti.Size = new System.Drawing.Size(87, 32);
            labellanchebalsti.TabIndex = 2;
            labellanchebalsti.Text = "Balanço Diario";
            this.tabPage2.Controls.Add(labellanchebalsti);
            double totalDiario = 0;
            for (int i = 0; i < bals.Count; i++)
            {
                var labellanchebals = new System.Windows.Forms.Label();
                labellanchebals.AutoSize = true;
                labellanchebals.BackColor = System.Drawing.SystemColors.Control;
                labellanchebals.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanchebals.ForeColor = System.Drawing.Color.Chocolate;
                labellanchebals.Location = new System.Drawing.Point(500, 91 + (i * 25));
                labellanchebals.Name = "balslanche" + i;
                labellanchebals.Size = new System.Drawing.Size(87, 32);
                labellanchebals.TabIndex = 2;
                labellanchebals.Text = pags.FirstOrDefault(x => x.Id == bals.ElementAt(i).IdPagamento).Nome;
                var labellanchebals2 = new System.Windows.Forms.Label();
                labellanchebals2.AutoSize = true;
                labellanchebals2.BackColor = System.Drawing.SystemColors.Control;
                labellanchebals2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanchebals2.ForeColor = System.Drawing.Color.LimeGreen;
                labellanchebals2.Location = new System.Drawing.Point(766, 91 + (i * 25));
                labellanchebals2.Name = "balslanchen" + i;
                labellanchebals2.Size = new System.Drawing.Size(87, 32);
                labellanchebals2.TabIndex = 2;
                var tot = bals.ElementAt(i).Total;
                totalDiario += tot;
                labellanchebals2.Text = tot.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
                this.tabPage2.Controls.Add(labellanchebals);
                this.tabPage2.Controls.Add(labellanchebals2);
                atual = this;
            }
            var labellanchebalstotal = new System.Windows.Forms.Label();
            labellanchebalstotal.AutoSize = true;
            labellanchebalstotal.BackColor = System.Drawing.SystemColors.Control;
            labellanchebalstotal.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labellanchebalstotal.ForeColor = System.Drawing.Color.Chocolate;
            labellanchebalstotal.Location = new System.Drawing.Point(500, 91 + (bals.Count * 25));
            labellanchebalstotal.Name = "balslanche" + bals.Count;
            labellanchebalstotal.Size = new System.Drawing.Size(87, 32);
            labellanchebalstotal.TabIndex = 2;
            labellanchebalstotal.Text = "Total";
            var labellanchebals2total = new System.Windows.Forms.Label();
            labellanchebals2total.AutoSize = true;
            labellanchebals2total.BackColor = System.Drawing.SystemColors.Control;
            labellanchebals2total.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labellanchebals2total.ForeColor = System.Drawing.Color.LimeGreen;
            labellanchebals2total.Location = new System.Drawing.Point(766, 91 + (bals.Count * 25));
            labellanchebals2total.Name = "balslanchen" + bals.Count;
            labellanchebals2total.Size = new System.Drawing.Size(87, 32);
            labellanchebals2total.TabIndex = 2;
            labellanchebals2total.Text = totalDiario.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
            this.tabPage2.Controls.Add(labellanchebalstotal);
            this.tabPage2.Controls.Add(labellanchebals2total);



            var balsmen = contexto.Balanco.Where(x => x.Dia.Month == hoje.Month && x.Dia.Year == hoje.Year).ToList().GroupBy(x => x.IdPagamento)
                .Select(n => new { Total=n.Sum(m => m.Total), IdPagamento = n.Key }); 
            var labellanchebalstimen = new System.Windows.Forms.Label();
            labellanchebalstimen.AutoSize = true;
            labellanchebalstimen.BackColor = System.Drawing.SystemColors.Control;
            labellanchebalstimen.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labellanchebalstimen.ForeColor = System.Drawing.Color.Chocolate;
            labellanchebalstimen.Location = new System.Drawing.Point(500, 425);
            labellanchebalstimen.Name = "balslanchetimen";
            labellanchebalstimen.Size = new System.Drawing.Size(87, 32);
            labellanchebalstimen.TabIndex = 2;
            labellanchebalstimen.Text = "Balanço Mensal";
            this.tabPage2.Controls.Add(labellanchebalstimen);
            double totalMensal = 0;
            for (int i = 0; i < bals.Count; i++)
            {
                var labellanchebalsmen = new System.Windows.Forms.Label();
                labellanchebalsmen.AutoSize = true;
                labellanchebalsmen.BackColor = System.Drawing.SystemColors.Control;
                labellanchebalsmen.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanchebalsmen.ForeColor = System.Drawing.Color.Chocolate;
                labellanchebalsmen.Location = new System.Drawing.Point(500, 450 + (i * 25));
                labellanchebalsmen.Name = "balslancheme" + i;
                labellanchebalsmen.Size = new System.Drawing.Size(87, 32);
                labellanchebalsmen.TabIndex = 2;
                labellanchebalsmen.Text = pags.FirstOrDefault(x => x.Id == balsmen.ElementAt(i).IdPagamento).Nome;
                var labellanchebals2men = new System.Windows.Forms.Label();
                labellanchebals2men.AutoSize = true;
                labellanchebals2men.BackColor = System.Drawing.SystemColors.Control;
                labellanchebals2men.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanchebals2men.ForeColor = System.Drawing.Color.LimeGreen;
                labellanchebals2men.Location = new System.Drawing.Point(766, 450 + (i * 25));
                labellanchebals2men.Name = "balslanchenmen" + i;
                labellanchebals2men.Size = new System.Drawing.Size(87, 32);
                labellanchebals2men.TabIndex = 2;
                var tot = balsmen.ElementAt(i).Total;
                totalMensal += tot;
                labellanchebals2men.Text = tot.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
                this.tabPage2.Controls.Add(labellanchebalsmen);
                this.tabPage2.Controls.Add(labellanchebals2men);
                atual = this;
            }
            var labellanchebalsmentotal = new System.Windows.Forms.Label();
            labellanchebalsmentotal.AutoSize = true;
            labellanchebalsmentotal.BackColor = System.Drawing.SystemColors.Control;
            labellanchebalsmentotal.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labellanchebalsmentotal.ForeColor = System.Drawing.Color.Chocolate;
            labellanchebalsmentotal.Location = new System.Drawing.Point(500, 450 + (bals.Count * 25));
            labellanchebalsmentotal.Name = "balslancheme" + bals.Count;
            labellanchebalsmentotal.Size = new System.Drawing.Size(87, 32);
            labellanchebalsmentotal.TabIndex = 2;
            labellanchebalsmentotal.Text = "Total";
            var labellanchebals2mentotal = new System.Windows.Forms.Label();
            labellanchebals2mentotal.AutoSize = true;
            labellanchebals2mentotal.BackColor = System.Drawing.SystemColors.Control;
            labellanchebals2mentotal.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labellanchebals2mentotal.ForeColor = System.Drawing.Color.LimeGreen;
            labellanchebals2mentotal.Location = new System.Drawing.Point(766, 450 + (bals.Count * 25));
            labellanchebals2mentotal.Name = "balslanchenmen" + bals.Count;
            labellanchebals2mentotal.Size = new System.Drawing.Size(87, 32);
            labellanchebals2mentotal.TabIndex = 2;
            labellanchebals2mentotal.Text = totalMensal.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
            this.tabPage2.Controls.Add(labellanchebalsmentotal);
            this.tabPage2.Controls.Add(labellanchebals2mentotal);
            atual = this;
        }
       public void atulizartabela()
        {
            try
            {
                var lanchesDia = contexto.LanchesDia.Where(x => x.Dia == DiaNormal).ToList();
                for (int i = 0; i < lanchesDia.Count; i++)
                {

                    this.tabPage1.Controls.Find("labellanchen" + i, false)[0].Text = lanchesDia.ElementAt(i).Qtd.ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

       public void atulizartabelad()
        {
            try
            {
                var lanches = contexto.Lanches.ToList();
                var lanchesmen = contexto.LanchesDia.Where(x => x.Dia.Month == DiaNormal.Month && x.Dia.Year == DiaNormal.Year).ToList().GroupBy(x => x.IdLanche)
                    .Select(n => new { Qtd = n.Sum(m => m.Qtd), IdLanche = n.Key }).ToList();
                double totallanches = 0;
                for (int i = 0; i < lanches.Count; i++)
                {
                    var tot = lanchesmen.FirstOrDefault(x => x.IdLanche == lanches.ElementAt(i).Id).Qtd;
                    totallanches += tot;
                    this.tabPage2.Controls.Find("labellanchenmen" + i, false)[0].Text = tot.ToString();
                }
                label2.Text = totallanches.ToString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }
        public void atulizartabelab()
        {
            try
            {
                var hoje = DiaNormal;
                var bals = contexto.Balanco.Where(x => x.Dia == hoje).ToList();
                double totalDiario = 0;
                for (int i = 0; i < bals.Count; i++)
                {
                    var tot = bals.ElementAt(i).Total;
                    totalDiario += tot;
                    this.tabPage2.Controls.Find("balslanchen" + i, false)[0].Text = tot.ToString("C2",CultureInfo.GetCultureInfo("pt-BR"));
                }
                this.tabPage2.Controls.Find("balslanchen" + bals.Count, false)[0].Text = totalDiario.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public void atulizartabelac()
        {
            try
            {
                var hoje = DiaNormal;
                var balsmen = contexto.Balanco.Where(x => x.Dia.Month == hoje.Month && x.Dia.Year == hoje.Year).ToList().GroupBy(x => x.IdPagamento)
                .Select(n => new { Total = n.Sum(m => m.Total), IdPagamento = n.Key }).ToList();
                double totalMensal = 0;
                for (int i = 0; i < balsmen.Count; i++)
                {
                    var tot = balsmen.ElementAt(i).Total;
                    totalMensal += tot;
                    this.tabPage2.Controls.Find("balslanchenmen" + i, false)[0].Text = tot.ToString("C2",CultureInfo.GetCultureInfo("pt-BR"));
                }
                this.tabPage2.Controls.Find("balslanchenmen" + balsmen.Count, false)[0].Text = totalMensal.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }
        void atualizar()
        {
            atulizartabela();
            atulizartabelab();
            atulizartabelac();
            atulizartabelad();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(Compra==null || Compra.IsDisposed)
                Compra = new PedidoForm();
            Compra.Show();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DiaNormal = contexto.LanchesDia.Max(x => x.Dia);
            DiaNormal = DiaNormal.Date < DateTime.Now.Date ? DateTime.Now.Date:DiaNormal.Date;
            DiaNormal = DiaNormal.AddDays(1);
            var lanches = new List<LancheDia>();
            foreach (var lanche in contexto.Lanches.ToList())
            {

                lanches.Add(new LancheDia() { IdLanche = lanche.Id, Dia = DiaNormal, Qtd = 0 });
            }
            lanches.ForEach(s => contexto.LanchesDia.Add(s));
            foreach (var pag in contexto.Pagamento.ToList())
            {
                var bal = Form1.contexto.Balanco.FirstOrDefault(x => x.IdPagamento == pag.Id && x.Dia == DiaNormal);
                if (bal == null)
                {
                    bal = Form1.contexto.Balanco.Add(new Balanco() { Total = 0, IdPagamento = pag.Id, Dia = DiaNormal });
                }
            }
            contexto.SaveChanges();
            atulizartabela();
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            atualizar();
        }

        

        private void button3_Click_1(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("você tem certeza que deseja voltar o dia ??",
                                     "Confirmar voltar Dia!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                contexto.Balanco.RemoveRange(contexto.Balanco.Where(x => x.Dia == DiaNormal));
                contexto.SaveChanges();
                contexto.LanchesDia.RemoveRange(contexto.LanchesDia.Where(x => x.Dia == DiaNormal));
                contexto.SaveChanges();
                DiaNormal = DiaNormal.AddDays(-1);
                atualizar();
            }
        }
    }
}
