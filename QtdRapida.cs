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
    public partial class QtdRapida : Form
    {
        public QtdRapida()
        {
            InitializeComponent();
        }
        private void QtdRapida_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Form1.contexto.Lanches.ToList().Select(x => x.Nome).ToArray());
            textBox1.Text = string.Empty;
            this.ActiveControl = textBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Nome.ToLower() == comboBox1.Text.ToLower());
            if (lan == null)
            {
                MessageBox.Show("Lanche inválido!");
                return;
            }
            int teste = 0;
            if (!int.TryParse(textBox1.Text,out teste))
            {
                MessageBox.Show("Quantidade inválida!");
                return;
            }
            int Qtd = int.Parse(textBox1.Text);
            var pedi = new PedidoLanche()
            {
                IdLanche = lan.Id,
                PontoCarne = "",
                Remover = "",
                Qtd = Qtd,
                Preco = Qtd * lan.Preco,
                Adicionais = ""
            };
            Form1.Compra.adicionarlinha(pedi);
            this.Close();
        }
        private void QtdRapida_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.F1:
                    comboBox1.SelectedIndex=0;
                    button1_Click(null, null);
                    break;
                case Keys.F2:
                    comboBox1.SelectedIndex = 1;
                    button1_Click(null, null);
                    break;
                case Keys.F3:
                    comboBox1.SelectedIndex = 2;
                    button1_Click(null, null);
                    break;
                case Keys.F4:
                    comboBox1.SelectedIndex = 3;
                    button1_Click(null, null);
                    break;
                case Keys.F5:
                    comboBox1.SelectedIndex = 4;
                    button1_Click(null, null);
                    break;
                case Keys.F6:
                    comboBox1.SelectedIndex = 5;
                    button1_Click(null, null);
                    break;
                case Keys.F7:
                    comboBox1.SelectedIndex = 6;
                    button1_Click(null, null);
                    break;
                case Keys.F8:
                    comboBox1.SelectedIndex = 7;
                    button1_Click(null, null);
                    break;
                case Keys.F9:
                    comboBox1.SelectedIndex = 8;
                    button1_Click(null, null);
                    break;
                case Keys.F10:
                    comboBox1.SelectedIndex = 9;
                    button1_Click(null, null);
                    break;
                case Keys.F11:
                    comboBox1.SelectedIndex = 10;
                    button1_Click(null, null);
                    break;
                case Keys.F12:
                    comboBox1.SelectedIndex = 11;
                    button1_Click(null, null);
                    break;
            }
        }

    }
}
