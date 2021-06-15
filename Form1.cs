using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private static DateTime DiaNormal = DateTime.Now.Date;
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
        void atualizar()
        {
            /*
            int lancheid = 0;
           if(int.TryParse(textBox1.Text,out lancheid))
            {
                var hoje = DiaNormal;
                var lanche= contexto.LanchesDia.FirstOrDefault(x => x.IdLanche == lancheid && x.Dia== hoje);
                if(lanche != null)
                {
                    lanche.Qtd += 1;
                    contexto.SaveChanges();
                    atulizartabela();
                }
                else
                {
                    MessageBox.Show("codigo lanche invalido");
                }
            }
            else
            {
                MessageBox.Show("lache invalido");
            }
            //textBox1.Text = "";
            */
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(Compra==null || Compra.IsDisposed)
                Compra = new PedidoForm();
            Compra.Show();
           // atualizar();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode==Keys.Enter)
            // atualizar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DiaNormal = contexto.LanchesDia.Max(x => x.Dia).AddDays(1);
            var lanches = new List<LancheDia>();
            foreach (var lanche in contexto.Lanches.ToList())
            {

                lanches.Add(new LancheDia() { IdLanche = lanche.Id, Dia = DiaNormal, Qtd = 0 });
            }
            lanches.ForEach(s => contexto.LanchesDia.Add(s));
            contexto.SaveChanges();
            atulizartabela();
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            var dev = contexto.Lanches.ToList();
            this.chart1.Series.Clear();
            foreach (var lanche in dev)
            {
                System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
                series1.ChartArea = "ChartArea1";
                series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                series1.BorderWidth = 12;
                
                series1.Legend = "Legend1";
                series1.Name = lanche.Nome;
                var lanchesdia = contexto.LanchesDia.Where(x => x.IdLanche == lanche.Id).ToList();
                foreach (var ponto in lanchesdia)
                    series1.Points.AddXY(ponto.Dia.Day, ponto.Qtd);
                series1.MarkerStyle = MarkerStyle.Circle;
                series1.MarkerSize = 13;
                this.chart1.Series.Add(series1);
            }
            /*
*/

        }
    }
}
