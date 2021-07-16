using System;
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
    public partial class ItemForm : Form
    {
        private List<PedidoLanchesAdicionais> pedLanchesAdicionaiss=new List<PedidoLanchesAdicionais>();
        public ItemForm()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void ItemForm_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Form1.contexto.Lanches.ToList().Select(x => x.Nome).ToArray());
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(Form1.contexto.Adicionais.ToList().Select(x => x.Nome).ToArray());
            comboBox2.Text = string.Empty;
            comboBox3.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Nome.ToLower() == comboBox1.Text.ToLower());
            if (lan == null)
            {
                MessageBox.Show("Lanche inválido");
                return;
            }
            var pedi = new PedidoLanche()
            {
                IdLanche = lan.Id,
                PontoCarne = comboBox2.Text,
                Remover = textBox3.Text,
                Preco = lan.Preco,
                Adicionais = "",
                pedLanchesAdicionais=pedLanchesAdicionaiss
            };
            if (pedLanchesAdicionaiss.Count > 0)
            {
                foreach (var a in pedLanchesAdicionaiss)
                {
                    pedi.Preco += a.Preco;
                    pedi.Adicionais += a.Nome + ", ";

                }
                pedi.Adicionais = pedi.Adicionais.Substring(0, pedi.Adicionais.Length - 2);
            }
            Form1.Compra.adicionarlinha(pedi);
            this.Close();
        }
        public void atualizar()
        {

            BindingSource bindingSourceMonth = new BindingSource();
            bindingSourceMonth.DataSource = Form1.contexto.Lanches.ToList();
            //indexcombo=ColumnMonth.Index;
            var source = new BindingSource();
            source.DataSource = pedLanchesAdicionaiss;
            dataGridView1.DataSource = source;

            dataGridView1.Columns["IdPedido"].Visible = false;
            dataGridView1.Columns["IdLanche"].Visible = false;
            dataGridView1.Columns["IdItem"].Visible = false;
            dataGridView1.Columns["IdAdicional"].Visible = false;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["Nome"].DisplayIndex = 0;
            dataGridView1.Columns["Nome"].Width = 220;
            foreach (DataGridViewBand band in dataGridView1.Columns)
            {
                band.ReadOnly = true;
            }
            dataGridView1.Refresh();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            PedidoLanchesAdicionais ped = new PedidoLanchesAdicionais();
            if (!string.IsNullOrEmpty(comboBox3.Text)) {
                ped.IdAdicional = Form1.contexto.Adicionais.FirstOrDefault(x => x.Nome == comboBox3.Text).Id;
                var dev = Form1.contexto.Adicionais.FirstOrDefault(x => x.Id == ped.IdAdicional);
                ped.Nome = dev.Nome;
                ped.Preco = dev.Preco;
                ped.Id = pedLanchesAdicionaiss.Count()+1;
                pedLanchesAdicionaiss.Add(ped);
                atualizar();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                pedLanchesAdicionaiss.RemoveAt(row.Index);
                atualizar();
            }
        }
    }
}
