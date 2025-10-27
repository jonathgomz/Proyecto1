using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculadora2
{
    public partial class Calculadora : Form
    {
        string connectionString = @"Server=.\sqlexpress;Database=CalculadoraDB;TrustServerCertificate=true;Integrated Security=SSPI;";
        double primer = 0;
        double segundo = 0;
        string operador = "";
        bool esperandoSegundoNumero = false; 

        public Calculadora()
        {
            InitializeComponent();
        }


        Clases.Csuma objS = new Clases.Csuma();
        Clases.Cresta objR = new Clases.Cresta();
        Clases.Cmultiplicar objM = new Clases.Cmultiplicar();
        Clases.Cdividir objD = new Clases.Cdividir();
        Clases.CRaiz objRa = new Clases.CRaiz();
        Clases.CPorcentaje objPo = new Clases.CPorcentaje();
        Clases.CCuadrado objCua = new Clases.CCuadrado();

        // evento para que se escriban los numeros
        private void btnNumero_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (esperandoSegundoNumero)
            {
                textBox1.Clear();
                esperandoSegundoNumero = false;
            }
            textBox1.Text += btn.Text;
        }

        private void btnSuma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) 
                return;

            primer = double.Parse(textBox1.Text);
            operador = "+";
            textBoxH.Text = $"{primer} {operador}";
            esperandoSegundoNumero = true;
        }

        private void btnResta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
               
                textBox1.Text = "-";
                return;
            }

            primer = double.Parse(textBox1.Text);
            operador = "-";
            textBoxH.Text = $"{primer} {operador}";
            esperandoSegundoNumero = true;
        }

        private void btnElevado_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            primer = double.Parse(textBox1.Text);
            double resultado = objCua.Potencia(primer);

            textBox1.Text = resultado.ToString();
            textBoxH.Text = $"{primer}² =";

            primer = resultado;
            operador = "";
            esperandoSegundoNumero = true;

        }

        private void btnMul_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            primer = double.Parse(textBox1.Text);
            operador = "*";
            textBoxH.Text = $"{primer} {operador}";
            esperandoSegundoNumero = true;
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) 
                return;

            primer = double.Parse(textBox1.Text);
            operador = "/";
            textBoxH.Text = $"{primer} {operador}";
            esperandoSegundoNumero = true;
        }

        private void btnRaiz_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;

            primer = double.Parse(textBox1.Text);
            operador = "√";
            double resultado = objRa.Raiz(primer);
            textBox1.Text = resultado.ToString();
            textBoxH.Text = $"√{primer} =";
            esperandoSegundoNumero = false;
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            if (string.IsNullOrEmpty(operador))
            {
                primer = double.Parse(textBox1.Text);
                double resultado = primer / 100;
                textBox1.Text = resultado.ToString();
                textBoxH.Text = $"{primer}% =";
            }
            else
            {
                segundo = double.Parse(textBox1.Text);
                double resultado = objPo.Porcentuar(primer, segundo, operador);
                textBox1.Text = resultado.ToString();
                textBoxH.Text = $"{primer} {operador} {segundo}%";
            }

            esperandoSegundoNumero = true;
        }

       
        private void btnE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) 
                return;

            segundo = double.Parse(textBox1.Text);
            double resultado = 0;

            switch (operador)
            {
                case "+":
                    resultado = objS.Sumar(primer, segundo);
                    break;
                case "-":
                    resultado = objR.Restar(primer, segundo);
                    break;
                case "*":
                    resultado = objM.Multiplicar(primer, segundo);
                    break;
                case "/":
                    if (segundo == 0)
                    {
                        MessageBox.Show("No se puede dividir entre 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Clear();
                        return;
                    }
                    resultado = objD.Dividir(primer, segundo);
                    break;
                case "%":
                    resultado = objPo.Porcentuar(primer, segundo, operador);
                    break;
            }

            textBox1.Text = resultado.ToString();
            textBoxH.Text = $"{primer} {operador} {segundo} =";
            primer = resultado;
            segundo = 0;
            operador = "";
            esperandoSegundoNumero = true;
            GuardarH(textBoxH.Text, resultado.ToString());
        }

        
        private void btnClearEntry_Click(object sender, EventArgs e) //Boton CE
        {
            textBox1.Clear(); //limpiar texto grande
            textBoxH.Clear();//limpiar historial
            primer = 0;
            segundo = 0;
            operador = "";
            esperandoSegundoNumero = false;
        }

        //imagen que borra
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;

            if (textBox1.Text.Length == 1)
                textBox1.Clear();
            else
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
        }

        //clases vacias que no puedo borrar
        private void Calculadora_Load(object sender, EventArgs e) { }
        private void textBoxH_TextChanged(object sender, EventArgs e) { }
        private void btn0_Click(object sender, EventArgs e)
        {}
        private void btn1_Click(object sender, EventArgs e)
        {}
        private void btn2_Click(object sender, EventArgs e)
        {}
        private void btn3_Click(object sender, EventArgs e)
        {}
        private void btn4_Click(object sender, EventArgs e)
        {}
        private void btn5_Click(object sender, EventArgs e)
        {}
        private void btn6_Click(object sender, EventArgs e)
        {}
        private void btn7_Click(object sender, EventArgs e)
        {}
        private void btn8_Click(object sender, EventArgs e)
        {}
        private void btn9_Click(object sender, EventArgs e)
        {}

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = "SELECT Operacion, Resultado, FechaHora FROM HistorialCalculos ORDER BY Id DESC";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    SqlDataReader reader = cmd.ExecuteReader();

                    StringBuilder historial = new StringBuilder();

                    while (reader.Read())
                    {
                        string operacion = reader["Operacion"].ToString();
                        string resultado = reader["Resultado"].ToString();
                        DateTime fechahora = Convert.ToDateTime(reader["FechaHora"]);

                        historial.AppendLine($"{operacion}  {resultado}         ({fechahora})");
                    }

                    if (historial.Length == 0)
                    {
                        MessageBox.Show("Historial vacío");
                    }
                    else
                    {
                        MessageBox.Show(historial.ToString(), "Historial de cálculos");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar historial: " + ex.Message);
            }
        }

        private void GuardarH(string operacion, string resultado)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = "INSERT INTO HistorialCalculos (Operacion, Resultado, FechaHora) VALUES (@Operacion, @Resultado, GETDATE())";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Operacion", operacion);
                    cmd.Parameters.AddWithValue("@Resultado", resultado);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar historial: " + ex.Message);
            }
        }


    }
}
