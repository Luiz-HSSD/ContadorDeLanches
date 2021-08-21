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
using System.Drawing.Printing;

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
        private PrintDocument printDocument1 = new PrintDocument();
        private string stringToPrint;
        private void PedidoForm_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Form1.contexto.Cliente.ToList().Select(x => x.Nome).ToArray());
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(Form1.contexto.Pagamento.ToList().Select(x => x.Nome).ToArray());
            comboBox4.Items.Clear();
            comboBox4.Items.AddRange(Form1.contexto.Pagamento.ToList().Select(x => x.Nome).ToArray());
            comboBox5.Items.Clear();
            comboBox5.Items.AddRange(Form1.contexto.Pagamento.ToList().Select(x => x.Nome).ToArray());
            dateTimePicker1.Value = DateTime.Now;
            comboBox2.SelectedItem = null;
            checkBox1.Checked = false;
            // Associate the PrintPage event handler with the PrintPage event.
            printDocument1.PrintPage +=
                new PrintPageEventHandler(printDocument1_PrintPage);
        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters
            // of stringToPrint that will fit within the bounds of the page.
            Rectangle folha = new Rectangle(0, 0, 414, 414);
            var fonte = new Font(new FontFamily("Arial"), 12); 
            e.Graphics.MeasureString(stringToPrint, fonte,
                folha.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, fonte, Brushes.Black,
                folha, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
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
            var pag1 = Form1.contexto.Pagamento.FirstOrDefault(x => x.Nome == comboBox3.Text);
            double valorPagoParcial = 0;
            if (pag1 == null)
            {
                MessageBox.Show("Favor inserir o pagamento");
                return;
            }
            else
            {
                valorPagoParcial += double.Parse(textBox1.Text, CultureInfo.GetCultureInfo("pt-BR"));
            }
            var pag2 = Form1.contexto.Pagamento.FirstOrDefault(x => x.Nome == comboBox4.Text);
            var valorpag2 = textBox2.Text;
            if (pag2 == null && !string.IsNullOrEmpty(valorpag2) && valorpag2 != "0" && valorpag2 != "0,0" && valorpag2 != "0,00" && checkBox2.Checked)
            {
                MessageBox.Show("Favor inserir o pagamento");
                return;
            }
            else
            {
                if (pag2 != null)
                    valorPagoParcial += double.Parse(textBox2.Text, CultureInfo.GetCultureInfo("pt-BR"));
            }
            var pag3 = Form1.contexto.Pagamento.FirstOrDefault(x => x.Nome == comboBox5.Text);
            var valorpag3 = textBox3.Text;
            if (pag3 == null && !string.IsNullOrEmpty(valorpag3) && valorpag3 != "0" && valorpag3 != "0,0" && valorpag3 != "0,00" && checkBox2.Checked)
            {
                MessageBox.Show("Favor inserir o pagamento");
                return;
            }
            else
            {
                if (pag3 != null)
                    valorPagoParcial += double.Parse(textBox3.Text, CultureInfo.GetCultureInfo("pt-BR"));
            }

            double totalped = 0;
            foreach (var d in lanchesped)
            {
                totalped += d.Preco;
            }
            if (valorPagoParcial!= totalped)
            {
                MessageBox.Show("Pagamento Diferente do total do pedido");
                return;
            }
            Form1.contexto.SaveChanges();
            var ValorPago1f = double.Parse(textBox1.Text, CultureInfo.GetCultureInfo("pt-BR")); 
            var ped = new Pedido()
            {
                Chegada = dateTimePicker1.Value,
                ParaViagem = checkBox1.Checked,
                IdCliente = cli.Id,
                IdPagamento1 = pag1.Id,
                ValorPago1 =ValorPago1f,
                ValorPago2=null,
                ValorPago3=null,
                status = comboBox2.SelectedIndex,
                Total = totalped
            };
            if (pag2 == null)
            {
                ped.IdPagamento2 = null;
                
            }
            else
            {
                ped.IdPagamento2 = pag2.Id;
                ped.ValorPago2 = double.Parse(textBox2.Text, CultureInfo.GetCultureInfo("pt-BR"));
            }
            if (pag3 == null)
            {
                ped.IdPagamento3 = null;

            }
            else
            {
                ped.IdPagamento3 = pag3.Id;
                ped.ValorPago3 = double.Parse(textBox3.Text, CultureInfo.GetCultureInfo("pt-BR"));
            }
            ped = Form1.contexto.Pedido.Add(ped);
            Form1.contexto.SaveChanges();
            foreach (var pedi in lanchesped)
            {
                pedi.IdPedido = ped.Id;
                Form1.contexto.PedidoLanche.Add(pedi);
                var DiaNormal = Form1.DiaNormal;
                var lanche = Form1.contexto.LanchesDia.FirstOrDefault(x => x.IdLanche == pedi.IdLanche && x.Dia == DiaNormal);
                if (lanche != null)
                {
                    lanche.Qtd += 1;
                    if (pedi.pedLanchesAdicionais.Count() > 0)
                    {
                        foreach (var a in pedi.pedLanchesAdicionais)
                        {
                            a.IdPedido = pedi.IdPedido;
                            Form1.contexto.PedidoLanchesAdicionais.Add(a);
                        }
                    }
                    Form1.contexto.SaveChanges();
                    Form1.atual.atulizartabela();
                }
                Form1.contexto.SaveChanges();
                
            }
            var hoje = Form1.DiaNormal;
            var bal = Form1.contexto.Balanco.FirstOrDefault(x => x.IdPagamento == ped.IdPagamento1 && x.Dia == hoje);
            if (bal == null)
            {
                bal = Form1.contexto.Balanco.Add(new Balanco() { Total = ped.ValorPago1, IdPagamento = ped.IdPagamento1.Value, Dia = Form1.DiaNormal });
            }
            else
            {
                bal.Total += ped.ValorPago1;
                var entry = Form1.contexto.Entry(bal); // Gets the entry for entity inside context
                entry.State = EntityState.Modified;
            }
            Form1.contexto.SaveChanges();
            if (pag2 != null)
            {
                var bal2 = Form1.contexto.Balanco.FirstOrDefault(x => x.IdPagamento == ped.IdPagamento2 && x.Dia == hoje);
                if (bal2 == null)
                {
                    bal2 = Form1.contexto.Balanco.Add(new Balanco() { Total = ped.ValorPago2.Value, IdPagamento = ped.IdPagamento2.Value, Dia = Form1.DiaNormal });
                }
                else
                {
                    bal2.Total += ped.ValorPago2.Value;
                    var entry = Form1.contexto.Entry(bal2); // Gets the entry for entity inside context
                    entry.State = EntityState.Modified;
                }
            }
            if (pag3 != null)
            {
                var bal3 = Form1.contexto.Balanco.FirstOrDefault(x => x.IdPagamento == ped.IdPagamento3 && x.Dia == hoje);
                if (bal3 == null)
                {
                    bal3 = Form1.contexto.Balanco.Add(new Balanco() { Total = ped.ValorPago3.Value, IdPagamento = ped.IdPagamento3.Value, Dia = Form1.DiaNormal });
                }
                else
                {
                    bal3.Total += ped.ValorPago3.Value;
                    var entry = Form1.contexto.Entry(bal3); // Gets the entry for entity inside context
                    entry.State = EntityState.Modified;
                }
            }
            Form1.contexto.SaveChanges();
            salvarPedido(ped, cli, pag1);
            this.Close();
        }
        public string Diretoriocomanda = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\comandas";
        private void salvarPedido(Pedido ped,Cliente cli,Pagamento pag)
        {
            var lanches = Form1.contexto.Lanches.ToList();
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
                comanda += pedi.IdItem + "\t\tItem:" + pedi.LancheNome + "\t\t Preço: " + lanches.FirstOrDefault(x => x.Id == pedi.IdLanche).Preco.ToString("C2",CultureInfo.GetCultureInfo("pt-BR"))+ "\r\n";
            }
            foreach (var pedi in lanchesped)
            {
                if (pedi.pedLanchesAdicionais.Count() > 0)
                {
                    foreach (var a in pedi.pedLanchesAdicionais)
                    {
                        comanda += (lanchesped.Count() + a.Id) + "\t\tItem Adicional:" + a.Nome + "\t\t Preço: " + a.Preco.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")) + "\r\n";
                    }
                }
            }
            comanda += "\r\n";
            comanda += "Total: " + label6.Text + "\r\n";
            var arquivo = Diretoriocomandahoje + "\\" + ped.Id + " " + DateTime.Now.ToString("HH-mm-ss")+".txt";
            File.WriteAllText(arquivo, comanda);
            if (checkBox3.Checked) {
                printDocument1.DocumentName = ped.Id + " " + DateTime.Now.ToString("HH-mm-ss") + ".txt";
                using (FileStream stream = new FileStream(arquivo, FileMode.Open))
                using (StreamReader reader = new StreamReader(stream))
                {
                    stringToPrint = reader.ReadToEnd();
                }
                printDocument1.Print();
            }
            //System.Diagnostics.Process.Start(arquivo);
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
            if (!checkBox2.Checked)
            {
                textBox1.Text = (total).ToString("0.00", CultureInfo.GetCultureInfo("pt-BR"));
            }
        }
        public void adicionarlinha(PedidoLanche ped)
        {
            ped.LancheNome = Form1.contexto.Lanches.FirstOrDefault(x => x.Id == ped.IdLanche).Nome;
            ped.IdItem = lanchesped.Count();
            if (ped.pedLanchesAdicionais.Count() > 0)
            {
                foreach (var a in ped.pedLanchesAdicionais)
                {
                    a.IdLanche = ped.IdLanche;
                    a.IdItem = ped.IdItem;
                }
            }
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
                case Keys.Divide:
                    checkBox2.Checked = !checkBox2.Checked;
                    break;
                case Keys.None:
                    if(e.KeyValue == 193)
                        checkBox2.Checked =! checkBox2.Checked;
                    break;
            }
        }
        private void PedidoForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void PedidoForm_KeyDown(object sender, KeyEventArgs e)
        {
            dev(e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                comboBox4.Visible = false;
                comboBox5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                calculatotal();
                textBox2.Text = "0,00";
                textBox3.Text = "0,00";
                comboBox4.Text = "";
                comboBox5.Text = "";
                comboBox4.SelectedIndex = -1;
                comboBox5.SelectedIndex = -1;
            }
            else
            {
                comboBox4.Visible = true;
                comboBox5.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
            }
        }
    }
}
