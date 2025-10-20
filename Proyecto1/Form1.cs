using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn0_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            string operacion = textBox1.Text;

            try
            {
                var resultado = new System.Data.DataTable().Compute(operacion, " ");
                textBox1.Text = resultado.ToString();
            }
            catch {
                textBox1.Text = "Error al realizar la operacion";
                   }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           textBox1.Clear();
        }
    }
}
