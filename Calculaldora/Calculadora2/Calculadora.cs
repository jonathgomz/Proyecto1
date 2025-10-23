using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculadora2
{
    public partial class Calculadora : Form
    {
        double primer;
        double segundo;
        string operador;
        public Calculadora()
        {
            InitializeComponent();
        }

        Clases.Csuma objS = new Clases.Csuma();
        Clases.Cresta objR = new Clases.Cresta();
        Clases.Cmultiplicar objM = new Clases.Cmultiplicar();
        Clases.Cdividir objD = new Clases.Cdividir();
        Clases.CRaiz objRa = new Clases.CRaiz();
        private void btn0_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "0";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "6";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "9";
        }

        private void btnSuma_Click(object sender, EventArgs e)
        {
            operador = "+";
            primer = double.Parse(textBox1.Text);
            textBox1.Clear();
        }

        private void btnResta_Click(object sender, EventArgs e)
        {
            operador = "-";
            primer = double.Parse(textBox1.Text);
            textBox1.Clear();
        }

        private void btnMul_Click(object sender, EventArgs e)
        {
            operador = "*";
            primer = double.Parse(textBox1.Text);
            textBox1.Clear();
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            operador = "/";
            primer = double.Parse(textBox1.Text);
            textBox1.Clear();
        }

        private void btnRaiz_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Ingrese un número primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            operador = "√";
            primer = double.Parse(textBox1.Text);
            textBox1.Clear();
        }

        private void Calculadora_Load(object sender, EventArgs e)
        {

        }

        private void btnE_Click(object sender, EventArgs e)
        {

            double resultado = 0;
            if (operador == "√")
            {
                resultado = objRa.Raiz(primer);
                textBox1 .Text = resultado.ToString();
                return;
            }


            segundo =  double.Parse(textBox1.Text);

            double Sum;
            double Res;
            double Mul;
            double Div;
            double Rai;

            switch(operador)
            {
                case "+":
                    Sum = objS.Sumar( (primer), (segundo) );
                    textBox1.Text = Sum.ToString();
                    break;

                case "-":
                    Res = objR.Restar((primer), (segundo));
                    textBox1.Text = Res.ToString();
                    break;

                case "*":
                    Mul = objM.Multiplicar((primer), (segundo));
                    textBox1.Text = Mul.ToString();
                    break;

                case "/":
                    Div = objD.Dividir((primer), (segundo));
                    textBox1.Text = Div.ToString();
                    break;

            }
        }

        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 1)
                textBox1.Text = "";
            else
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
        }


    }
}
