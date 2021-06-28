using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ContadorDeLanches
{
    public partial class PedidoForm : Form
    {
        private static ItemForm Compra = new ItemForm();
        internal static List<PedidoLanche> lanchesped;
        public PedidoForm()
        {
            InitializeComponent();
            lanchesped = new List<PedidoLanche>() { };
            atualizar();
        }

        private void PedidoForm_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Form1.contexto.Cliente.ToList().Select(x => x.Nome).ToArray());
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(Form1.contexto.Pagamento.ToList().Select(x => x.Nome).ToArray());
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
        public void fecharPedido()
        {
            var cli = Form1.contexto.Cliente.FirstOrDefault(x => x.Nome == comboBox1.Text);
            if (cli == null)
            {
                cli = Form1.contexto.Cliente.Add(new Cliente() { Nome = comboBox1.Text });
            }
            var pag = Form1.contexto.Pagamento.FirstOrDefault(x => x.Nome == comboBox3.Text);
            if (pag == null)
            {
                if (comboBox3.Text == string.Empty || comboBox3.Text == null)
                    pag = Form1.contexto.Pagamento.FirstOrDefault(x => x.Id == 1);
                else
                    pag = Form1.contexto.Pagamento.Add(new Pagamento() { Nome = comboBox3.Text });
            }

            Form1.contexto.SaveChanges();
            double totalped = 0;
            foreach (var d in lanchesped)
            {
                totalped += d.Preco;
            }
            var ped = new Pedido()
            {
                Chegada = dateTimePicker1.Value,
                ParaViagem = checkBox1.Checked,
                IdCliente = cli.Id,
                IdPagamento = pag.Id,
                status = comboBox2.SelectedIndex,
                Total = totalped
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
                
            }
            var hoje = Form1.contexto.LanchesDia.Max(x => x.Dia);
            var bal = Form1.contexto.Balanco.FirstOrDefault(x => x.IdPagamento == ped.IdPagamento && x.Dia == hoje);
            if (bal == null)
            {
                bal = Form1.contexto.Balanco.Add(new Balanco() { Total = ped.Total, IdPagamento = ped.IdPagamento, Dia = DateTime.Now.Date });
            }
            else
            {
                bal.Total += ped.Total;
                var entry = Form1.contexto.Entry(bal); // Gets the entry for entity inside context
                entry.State = EntityState.Modified;
            }
            Form1.contexto.SaveChanges();
            salvarPedido(ped, cli, pag);
            this.Close();
        }
        public string Diretoriocomanda = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\comandas";
        private void salvarPedido(Pedido ped,Cliente cli,Pagamento pag)
        {
            if (!Directory.Exists(Diretoriocomanda))
            {
                Directory.CreateDirectory(Diretoriocomanda);
            }
            var Diretoriocomandahoje = Diretoriocomanda + "\\"+DateTime.Now.ToString("dd-MM-yyyy");
            if (!Directory.Exists(Diretoriocomandahoje))
            {
                Directory.CreateDirectory(Diretoriocomandahoje);
            }
            var comanda = "";
            comanda += "Pedido: "+ped.Id+ "\r\n";
            comanda += "Cliente: "+cli.Nome+ "\r\n";
            comanda += "Chegada: "+ped.Chegada.ToString("dd/MM/yyyy HH:mm:ss")+ "\r\n";
            comanda += "Pagamento: "+ pag.Nome+ "\r\n";
            comanda += "Para Viagem: "+ (ped.ParaViagem?"Sim":"Não")+ "\r\n";
            comanda += "\r\n";
            foreach (var pedi in lanchesped)
            {
                comanda += pedi.IdItem + "\t\tItem:" + pedi.LancheNome + "\t\t Preço: " + pedi.Preco.ToString("C2",CultureInfo.GetCultureInfo("pt-BR"))+ "\r\n";
            }
            comanda += "\r\n";
            comanda += "Total: " + label6.Text + "\r\n";
            var arquivo = Diretoriocomandahoje + "\\" + ped.Id + " " + DateTime.Now.ToString("HH-mm-ss")+".txt";
            File.WriteAllText(arquivo, comanda);
            System.Diagnostics.Process.Start(arquivo);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fecharPedido();
        }
        public void calculatotal()
        {
            double total = 0;
            foreach (var d in lanchesped)
            {
                total += d.Preco;
            }
            label6.Text = (total).ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
        }
        public void adicionarlinha(PedidoLanche ped)
        {
            ped.LancheNome = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == ped.IdLanche).Nome;
            ped.IdItem = lanchesped.Count();
            lanchesped.Add(ped);
            atualizar();
            calculatotal();
        }
        public void atualizar()
        {
           
            BindingSource bindingSourceMonth = new BindingSource();
            bindingSourceMonth.DataSource = Form1.contexto.Lanches.ToList();
            //indexcombo=ColumnMonth.Index;
            var source = new BindingSource();
            source.DataSource = lanchesped;
            dataGridView1.DataSource = source;
           
            dataGridView1.Columns["IdPedido"].Visible = false;
            dataGridView1.Columns["IdLanche"].Visible = false;
            dataGridView1.Columns["IdItem"].Visible = false;
            dataGridView1.Columns["LancheNome"].DisplayIndex = 0;
            dataGridView1.Columns["LancheNome"].Width = 220;
            foreach (DataGridViewBand band in dataGridView1.Columns)
            {
                band.ReadOnly = true;
            }
            dataGridView1.Refresh();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                lanchesped.RemoveAt(row.Index);
                atualizar();
                calculatotal();
            }
        }
        public void dev(KeyEventArgs e)
        {
            var lan = new Lanche();
            var pedi = new PedidoLanche()
            {
                PontoCarne = "",
                Adicionais = "",
                Remover = ""
            };
            switch (e.KeyCode)
            {
                case Keys.F1:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 1);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F2:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 2);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F3:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 3);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F4:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 4);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F5:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 5);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F6:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 6);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F7:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 7);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F8:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 8);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F9:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 9);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F10:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 10);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F11:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 11);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.F12:
                    lan = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == 12);
                    pedi.IdLanche = lan.Id;
                    pedi.Preco = lan.Preco;
                    Form1.Compra.adicionarlinha(pedi);
                    break;
                case Keys.D1:
                    comboBox3.SelectedIndex = comboBox3.FindStringExact("Dinheiro");
                    fecharPedido();
                    break;
                case Keys.D2:
                    comboBox3.SelectedIndex = comboBox3.FindStringExact("cc - Cartão de Creditto");
                    fecharPedido();
                    break;
                case Keys.D3:
                    comboBox3.SelectedIndex = comboBox3.FindStringExact("cd - Cartão de Débito");
                    fecharPedido();
                    break;
                case Keys.D4:
                    comboBox3.SelectedIndex = comboBox3.FindStringExact("pix");
                    fecharPedido();
                    break;
            }
        }
        private void PedidoForm_KeyPress(object sender, KeyPressEventArgs e)
        {
           // dev(e);


        }

        private void PedidoForm_KeyDown(object sender, KeyEventArgs e)
        {
           // e.
            dev(e);
        }
    }
}
