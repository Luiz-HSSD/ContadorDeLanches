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
    public partial class PedidoForm : Form
    {
        private static ItemForm Compra = new ItemForm();
        internal static List<PedidoLanche> lanchesped;
        public PedidoForm()
        {
            InitializeComponent();
            var source = new BindingSource();
            lanchesped = new List<PedidoLanche>() {  };
            source.DataSource = lanchesped;
            dataGridView1.DataSource = source;
        }

        private void PedidoForm_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Form1.contexto.Cliente.ToList().Select(x=>x.Nome).ToArray());
            dateTimePicker1.Value = DateTime.Now;
            comboBox2.SelectedItem = null;
            checkBox1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Compra == null || Compra.IsDisposed)
                Compra = new ItemForm();
            Compra.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var cli = Form1.contexto.Cliente.FirstOrDefault(x => x.Nome == comboBox1.Text);
            if (cli == null)
            {
                cli = Form1.contexto.Cliente.Add(new Cliente() { Nome = comboBox1.Text });
            }
            var ped = new Pedido()
            {
                Chegada = dateTimePicker1.Value,
                ParaViagem = checkBox1.Checked,
                IdCliente = cli.Id,
                status = comboBox2.SelectedIndex,
            };
            ped = Form1.contexto.Pedido.Add(ped);
            Form1.contexto.SaveChanges();
            foreach (var pedi in lanchesped)
            {
                pedi.IdPedido = ped.Id;
                Form1.contexto.PedidoLanche.Add(pedi);
                var DiaNormal = Form1.contexto.LanchesDia.Max(x => x.Dia);
                var lanche = Form1.contexto.LanchesDia.FirstOrDefault(x => x.IdLanche == pedi.IdLanche && x.Dia == DiaNormal);
                if (lanche != null)
                {
                    lanche.Qtd += 1;
                    Form1.contexto.SaveChanges();
                    Form1.atual.atulizartabela();
                }
                Form1.contexto.SaveChanges();
                this.Close();

            }
        }
        public void adicionarlinha(PedidoLanche ped)
        {
            ped.LancheNome = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == ped.IdLanche).Nome;
            lanchesped.Add(ped);
            
            var source = new BindingSource();
            source.DataSource = lanchesped;
            dataGridView1.DataSource = source;
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                lanchesped.RemoveAt(row.Index);
                var source = new BindingSource();
                source.DataSource = lanchesped;
                dataGridView1.DataSource = source;
                dataGridView1.Refresh();
            }
        }
    }
}
