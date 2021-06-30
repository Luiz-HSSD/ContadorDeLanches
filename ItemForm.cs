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
            comboBox2.Text = string.Empty;
            textBox2.Text = string.Empty;
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
                MessageBox.Show("Lanche invalido");
                return;
            }
            var pedi = new PedidoLanche()
            {
                IdLanche=lan.Id,
                PontoCarne = comboBox2.Text,
                Adicionais = textBox2.Text,
                Remover = textBox3.Text,
                Preco=lan.Preco,
                
            };
            Form1.Compra.adicionarlinha(pedi);
            this.Close();
        }
    }
}
