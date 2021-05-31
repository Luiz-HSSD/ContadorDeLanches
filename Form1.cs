﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContadorDeLanches
{
    public partial class Form1 : Form
    {
        internal static LancheContexto contexto = new LancheContexto();
        private static DateTime DiaNormal = DateTime.Now.Date;
        public Form1()
        {
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
                this.Controls.Add(labellanche);
                var labellanche2 = new System.Windows.Forms.Label();
                labellanche2.AutoSize = true;
                labellanche2.BackColor = System.Drawing.SystemColors.Control;
                labellanche2.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labellanche2.ForeColor = System.Drawing.Color.LimeGreen;
                labellanche2.Location = new System.Drawing.Point(230, 46+(i*42));
                labellanche2.Name = "labellanchen"+i;
                labellanche2.Size = new System.Drawing.Size(87, 32);
                labellanche2.TabIndex = 2;
                labellanche2.Text = lanchesDia.ElementAt(i).Qtd.ToString();
                this.Controls.Add(labellanche);
                this.Controls.Add(labellanche2);
            }
            InitializeComponent();
        }
        void atulizartabela()
        {
            var lanchesDia = contexto.LanchesDia.Where(x => x.Dia == DiaNormal).ToList();
            for (int i = 0; i < lanchesDia.Count; i++)
            {

                this.Controls.Find("labellanchen" + i,false)[0].Text= lanchesDia.ElementAt(i).Qtd.ToString();
            }
        }
        void atualizar()
        {
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
            textBox1.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            atualizar();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
             atualizar();
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
    }
}
